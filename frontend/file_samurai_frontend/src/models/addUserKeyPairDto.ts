export interface AddUserKeyPairDto {
    userId: string;
    publicKey: string;
    privateKey: string;
    nonce: string;
    salt: string;
}