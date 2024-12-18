export interface AddOrGetUserFileAccessDto {
    encryptedFileKey: string;
    role: string;
    userId: string;
    fileId: string;
}
