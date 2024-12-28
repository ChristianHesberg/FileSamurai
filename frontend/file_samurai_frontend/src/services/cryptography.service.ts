import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {
    createCipheriv,
    createDecipheriv,
    generateKeyPairSync, pbkdf2Sync,
    privateDecrypt,
    publicEncrypt,
    randomBytes
} from "node:crypto";
import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {DecryptionError} from "../errors/decryption.error";

export class CryptographyService {
    encryptAes256Gcm(plaintext: Buffer, key: Buffer): AesGcmEncryptionOutput {
        const nonce = randomBytes(12);
        const cipher = createCipheriv('aes-256-gcm', key, nonce);

        const cipherTextBuffer = Buffer.concat([
            cipher.update(plaintext),
            cipher.final()
        ]);
        const tag = cipher.getAuthTag();

        return {
            cipherText: cipherTextBuffer.toString('base64'),
            nonce: nonce.toString('base64'),
            tag: tag.toString('base64')
        };
    }

    decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: Buffer): Buffer {
        const { cipherText, nonce, tag } = encryptedData;
        try{
            const decipher = createDecipheriv('aes-256-gcm', key, Buffer.from(nonce, 'base64'));
            decipher.setAuthTag(Buffer.from(tag, 'base64'));

            return Buffer.concat([
                decipher.update(Buffer.from(cipherText, 'base64')),
                decipher.final()
            ]);
        } catch (error) {
            throw new DecryptionError();
        }
    }

    generateRsaKeyPair(): RsaKeyPairModel {
        const { publicKey, privateKey } = generateKeyPairSync('rsa', {
            modulusLength: 2048,
            publicKeyEncoding: {
                type: 'spki',
                format: 'pem'
            },
            privateKeyEncoding: {
                type: 'pkcs8',
                format: 'pem'
            }
        });

        console.log(publicKey);

        return {
            private_key: privateKey,
            public_key: publicKey
        };
    }

    generateRsaKeyPairWithEncryption(password: string, saltSize: number = 16): EncryptedRsaKeyPairModel {
        const salt = randomBytes(saltSize);
        const key = this.deriveKeyFromPassword(password, salt);
        const { private_key, public_key } = this.generateRsaKeyPair();
        const { cipherText, nonce, tag } = this.encryptAes256Gcm(Buffer.from(private_key), key);
        return {
            privateKey: cipherText,
            publicKey: public_key,
            nonce: nonce,
            tag: tag,
            salt: salt.toString('base64')
        }
    }

    encryptWithPublicKey(data: Buffer, publicKey: string): Buffer {
        return publicEncrypt(publicKey, data);
    }

    decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Buffer {
        return privateDecrypt(privateKey, encryptedData);
    }

    deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Buffer {
        return pbkdf2Sync(password, salt, 100000, keyLength, 'sha512');
    }

    generateKey(size: number = 12): Buffer {
        return randomBytes(size);
    }

    decryptPrivateKey(privateKey: UserPrivateKeyDto, password: string): Buffer{
        const aesInput = {
            cipherText: privateKey.privateKey,
            nonce: privateKey.nonce,
            tag: privateKey.tag
        }
        return this.decryptAes256Gcm(
            aesInput,
            this.deriveKeyFromPassword(password, Buffer.from(privateKey.salt, 'base64'))
        );
    }
}