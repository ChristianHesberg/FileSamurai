import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {DecryptionError} from "../errors/decryption.error";
import {Buffer} from "buffer";
import {CryptographyServiceInterface} from "./cryptography.service.interface";

export class ClientSideCryptographyService {
    async encryptAes256Gcm(plaintext: Buffer, key: Buffer): Promise<AesGcmEncryptionOutput> {
        const cryptoKey = await window.crypto.subtle.importKey(
            'raw',
            key,
            { name: 'AES-GCM' },
            false,
            ['encrypt']
        );

        const nonce = window.crypto.getRandomValues(new Uint8Array(12));

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

    async decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: Buffer): Promise<Buffer> {
        try {
            const { cipherText, nonce } = encryptedData;

            const cryptoKey = await window.crypto.subtle.importKey(
                'raw',
                key,
                { name: 'AES-GCM' },
                false,
                ['decrypt']
            );

            // Decrypt the data
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

    async deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Promise<Buffer> {
        const encodedPassword = Buffer.from(password, "base64");

        const passwordKey = await window.crypto.subtle.importKey(
            'raw',
            encodedPassword,
            'PBKDF2',
            false,
            ['deriveKey']
        );

        const derivedKey = await window.crypto.subtle.deriveKey(
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
            true,
            ['encrypt', 'decrypt']
        );

        const derivedKeyBuffer = await window.crypto.subtle.exportKey('raw', derivedKey);
        console.log(derivedKeyBuffer.byteLength);
        return Buffer.from(derivedKeyBuffer);
    }
}