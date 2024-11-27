import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {createCipheriv, createDecipheriv, generateKeyPairSync, pbkdf2Sync, randomBytes} from "node:crypto";
import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";

export function generateRsaKeyPair(): RsaKeyPairModel {
    const { publicKey, privateKey } = generateKeyPairSync('rsa', {
        modulusLength: 2048,
        publicKeyEncoding: {
            type: 'spki',
            format: 'pem'
        },
        privateKeyEncoding: {
            type: 'pkcs8',
            format: 'der'
        }
    });

    return {
        private_key: privateKey,
        public_key: publicKey
    };
}

export function generateRsaKeyPairWithEncryption(password: string, saltSize: number = 16): EncryptedRsaKeyPairModel {
    const salt = randomBytes(saltSize);
    const key = deriveKeyFromPassword(password, salt);
    const { private_key, public_key } = generateRsaKeyPair();
    const { cipherText, nonce, tag } = encrypt(private_key, key);
    return {
        privateKey: cipherText,
        publicKey: public_key,
        nonce: nonce,
        tag: tag,
        salt: salt.toString('base64')
    }
}


function deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Buffer {
    return pbkdf2Sync(password, salt, 100000, keyLength, 'sha512');
}

function encrypt(plaintext: Buffer, key: Buffer): AesGcmEncryptionOutput {
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

function decrypt(encryptedData: AesGcmEncryptionOutput, key: Buffer): Buffer {
    const { cipherText, nonce, tag } = encryptedData;

    const decipher = createDecipheriv('aes-256-gcm', key, Buffer.from(nonce, 'base64'));
    decipher.setAuthTag(Buffer.from(tag, 'base64'));

    return Buffer.concat([
        decipher.update(Buffer.from(cipherText, 'base64')),
        decipher.final()
    ]);
}
//let obj = generateRsaKeyPairWithEncryption('secret');

/*console.log(JSON.stringify({
    id: 'id',
    privateKey: obj.privateKey,
    publicKey: obj.publicKey,
    nonce: obj.nonce,
    tag: obj.tag,
    salt: obj.salt,
}));*/
