import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {DecryptionError} from "../errors/decryption.error";
import {Buffer} from "buffer";
import {ICryptographyService} from "./cryptography.service.interface";
import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";

export class CryptographyService implements ICryptographyService {
    async encryptAes256Gcm(plaintext: Buffer, key: CryptoKey): Promise<AesGcmEncryptionOutput> {
        const nonce = window.crypto.getRandomValues(new Uint8Array(12));

        const encryptedData = await window.crypto.subtle.encrypt(
            {
                name: 'AES-GCM',
                iv: nonce,
                tagLength: 128,
            },
            key,
            plaintext
        );

        return {
            cipherText: Buffer.from(encryptedData).toString('base64'),
            nonce: Buffer.from(nonce).toString('base64')
        };
    }

    async encryptAes256GcmWithBufferKey(plaintext: Buffer, key: Buffer): Promise<AesGcmEncryptionOutput> {
        const nonce = window.crypto.getRandomValues(new Uint8Array(12));

        const cryptoKey = await window.crypto.subtle.importKey(
            'raw',
            key,
            {name: 'AES-GCM'},
            false,
            ['encrypt']
        );

        const encryptedData = await window.crypto.subtle.encrypt(
            {
                name: 'AES-GCM',
                iv: nonce,
                tagLength: 128,
            },
            cryptoKey,
            plaintext
        );

        return {
            cipherText: Buffer.from(encryptedData).toString('base64'),
            nonce: Buffer.from(nonce).toString('base64')
        };
    }

    async decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: CryptoKey): Promise<Buffer> {
        try {
            const {cipherText, nonce} = encryptedData;

            const decryptedData = await window.crypto.subtle.decrypt(
                {
                    name: 'AES-GCM',
                    iv: Buffer.from(nonce, 'base64'),
                    tagLength: 128,
                },
                key,
                Buffer.from(cipherText, "base64")
            );

            return Buffer.from(decryptedData);
        } catch (error) {
            throw new DecryptionError('Decryption failed');
        }
    }

    async decryptAes256GcmWithBufferKey(encryptedData: AesGcmEncryptionOutput, key: Buffer): Promise<Buffer> {
        try {
            const {cipherText, nonce} = encryptedData;

            const cryptoKey = await window.crypto.subtle.importKey(
                'raw',
                key,
                {name: 'AES-GCM'},
                false,
                ['decrypt']
            );

            const decryptedData = await window.crypto.subtle.decrypt(
                {
                    name: 'AES-GCM',
                    iv: Buffer.from(nonce, 'base64'),
                    tagLength: 128,
                },
                cryptoKey,
                Buffer.from(cipherText, "base64")
            );

            return Buffer.from(decryptedData);
        } catch (error) {
            throw new DecryptionError('Decryption failed');
        }
    }

    //key is generated with salt. Salt needs to be saved with encrypted data
    async deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Promise<CryptoKey> {
        const encodedPassword = Buffer.from(password, 'utf8');

        const passwordKey = await window.crypto.subtle.importKey(
            'raw',
            encodedPassword,
            'PBKDF2',
            false,
            ['deriveKey']
        );

        return await window.crypto.subtle.deriveKey(
            {
                name: 'PBKDF2',
                salt: salt,
                iterations: 100000,
                hash: 'SHA-512'
            },
            passwordKey,
            {
                name: 'AES-GCM',
                length: keyLength * 8
            },
            false,
            ['encrypt', 'decrypt']
        );
    }

    async hashPassword(password: string, salt: Buffer): Promise<Buffer> {
        const encodedPassword = Buffer.from(password, 'utf8');

        const passwordKey = await window.crypto.subtle.importKey(
            'raw',
            encodedPassword,
            'PBKDF2',
            false,
            ['deriveBits']
        );

        const derivedBits = await window.crypto.subtle.deriveBits(
            {
                name: 'PBKDF2',
                salt: salt,
                iterations: 100000,
                hash: 'SHA-512'
            },
            passwordKey,
            512
        );
        //const a = await window.crypto.subtle.exportKey("raw", derivedBits)
        return Buffer.from(derivedBits)
    }

    async generateRsaKeyPair(): Promise<RsaKeyPairModel> {
        const keyPair = await crypto.subtle.generateKey(
            {
                name: 'RSA-OAEP',
                modulusLength: 2048,
                publicExponent: new Uint8Array([0x01, 0x00, 0x01]), // 65537
                hash: 'SHA-256'
            },
            true,
            ['encrypt', 'decrypt']
        );

        return {
            public_key: await this.exportKeyToBase64(keyPair.publicKey),
            private_key: await this.exportKeyToBase64(keyPair.privateKey)
        };
    }

    async generateRsaKeyPairWithEncryption(key: CryptoKey, salt: Buffer): Promise<EncryptedRsaKeyPairModel> {
        const {private_key, public_key} = await this.generateRsaKeyPair();
        const {cipherText, nonce} = await this.encryptAes256Gcm(Buffer.from(private_key, 'base64'), key);

        return {
            privateKey: cipherText,
            publicKey: public_key,
            nonce: nonce,
            salt: salt.toString('base64')
        }
    }

    async encryptWithPublicKey(data: Buffer, publicKey: string): Promise<Buffer> {
        const converted = await this.importKeyFromBase64(publicKey, 'public');
        const encryptedMessage = await crypto.subtle.encrypt(
            {
                name: 'RSA-OAEP'
            },
            converted,
            data
        );
        return Buffer.from(encryptedMessage);
    }

    async decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Promise<Buffer> {
        const converted = await this.importKeyFromBuffer(privateKey, 'private');
        const decryptedMessage = await crypto.subtle.decrypt(
            {
                name: 'RSA-OAEP'
            },
            converted,
            encryptedData
        );
        return Buffer.from(decryptedMessage);
    }

    async decryptPrivateKey(privateKey: UserPrivateKeyDto, cryptoKey: CryptoKey): Promise<Buffer> {
        //const derivedKey = await this.deriveKeyFromPassword(password, Buffer.from(privateKey.salt, 'base64'));
        const aesInput = {
            cipherText: privateKey.privateKey,
            nonce: privateKey.nonce,
        }
        return await this.decryptAes256Gcm(
            aesInput,
            cryptoKey
        );
    }

    async generateKey(size: number): Promise<Buffer> {
        return Buffer.from(window.crypto.getRandomValues(new Uint8Array(size)));
    }

    private async exportKeyToBase64(key: CryptoKey): Promise<string> {
        const exported = await crypto.subtle.exportKey(
            key.type === 'private' ? 'pkcs8' : 'spki',
            key
        );
        return Buffer.from(exported).toString('base64');
    }

    private async importKeyFromBase64(base64Key: string, type: 'public' | 'private'): Promise<CryptoKey> {
        const binaryKey = Buffer.from(base64Key, 'base64');
        const format = type === 'private' ? 'pkcs8' : 'spki';
        return crypto.subtle.importKey(
            format,
            binaryKey,
            {
                name: 'RSA-OAEP',
                hash: 'SHA-256'
            },
            true,
            type === 'private' ? ['decrypt'] : ['encrypt']
        );
    }

    private async importKeyFromBuffer(key: Buffer, type: 'public' | 'private'): Promise<CryptoKey> {
        const format = type === 'private' ? 'pkcs8' : 'spki';
        return crypto.subtle.importKey(
            format,
            key,
            {
                name: 'RSA-OAEP',
                hash: 'SHA-256'
            },
            true,
            type === 'private' ? ['decrypt'] : ['encrypt']
        );
    }
}