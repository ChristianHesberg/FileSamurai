export interface EncryptedRsaKeyPairModel {
    privateKey: string;
    publicKey: string;
    nonce: string;
    tag: string;
    salt: string;
}