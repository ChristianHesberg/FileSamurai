export interface AddUserKeyPairDto {
    userId: string;
    publicKey: string;
    privateKey: string;
    nonce: string;
    tag: string;
    salt: string;
}