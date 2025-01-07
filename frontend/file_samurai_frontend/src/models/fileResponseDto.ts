export interface FileResponseDto {
    userFileAccess: {
        encryptedFileKey: string;
        role: string;
        userId: string;
        fileId: string;
    };
    file: {
        id: string;
        fileContents: string;
        nonce: string; 
        title: string;
    };
}