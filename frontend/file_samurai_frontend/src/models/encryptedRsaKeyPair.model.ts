export interface EncryptedRsaKeyPairModel {
    encryptedPrivateKey: string;
    publicKey: string;
    nonce: string;
    tag: string;
    salt: string;
}