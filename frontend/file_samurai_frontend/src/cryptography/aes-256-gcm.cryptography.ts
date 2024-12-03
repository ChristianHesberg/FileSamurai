import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {createCipheriv, createDecipheriv, randomBytes} from "node:crypto";

export function encryptAes256Gcm(plaintext: Buffer, key: Buffer): AesGcmEncryptionOutput {
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

export function decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: Buffer): Buffer {
    const { cipherText, nonce, tag } = encryptedData;

    const decipher = createDecipheriv('aes-256-gcm', key, Buffer.from(nonce, 'base64'));
    decipher.setAuthTag(Buffer.from(tag, 'base64'));

    return Buffer.concat([
        decipher.update(Buffer.from(cipherText, 'base64')),
        decipher.final()
    ]);
}