export interface AddUserFileAccessDto {
    encryptedFileKey: string;
    role: string;
    userId: string;
    fileId: string;
}
