export interface EncryptedRsaKeyPairModel {
    privateKey: string;
    publicKey: string;
    nonce: string;
    salt: string;
}