export interface AesGcmEncryptionOutput {
    cipherText: string;
    nonce: string;
    tag?: string;
}