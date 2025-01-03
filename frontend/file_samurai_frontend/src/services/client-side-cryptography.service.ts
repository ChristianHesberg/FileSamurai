import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {DecryptionError} from "../errors/decryption.error";
import {CryptographyServiceInterface} from "./cryptography.service.interface";

export class ClientSideCryptographyService {
    async encryptAes256Gcm(plaintext: string, key: string): Promise<AesGcmEncryptionOutput> {
        const encoder = new TextEncoder();
        const encodedPlaintext = encoder.encode(plaintext);
        const encodedKey = encoder.encode(key);

        const cryptoKey = await window.crypto.subtle.importKey(
            'raw',
            encodedKey,
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
            encodedPlaintext
        );

        const cipherTextBuffer = new Uint8Array(encryptedData);

        const decoder = new TextDecoder();
        return {
            cipherText: decoder.decode(cipherTextBuffer),
            nonce: decoder.decode(nonce),
        };
    }

    async decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: string): Promise<string> {
        try {
            const { cipherText, nonce } = encryptedData;

            const decoder = new TextDecoder();
            const encoder = new TextEncoder()
            const encodedKey = encoder.encode(key);
            const cipherTextArray = encoder.encode(cipherText);
            const nonceArray = encoder.encode(nonce);
            const cryptoKey = await window.crypto.subtle.importKey(
                'raw',
                encodedKey,
                { name: 'AES-GCM' },
                false,
                ['decrypt']
            );

            // Decrypt the data
            const decryptedData = await window.crypto.subtle.decrypt(
                {
                    name: 'AES-GCM',
                    iv: nonceArray,
                    tagLength: 128,
                },
                cryptoKey,
                cipherTextArray
            );

            return decoder.decode(decryptedData);
        } catch (error) {
            throw new DecryptionError('Decryption failed');
        }
    }
}