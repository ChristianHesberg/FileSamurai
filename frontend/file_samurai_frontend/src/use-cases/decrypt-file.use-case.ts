import type {FileService} from "../services/file.service";
import {FileResponseDto} from "../models/fileResponseDto";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import type {KeyService} from "../services/key.service";
import {decryptPrivateKey} from "../cryptography/utils.cryptography";
import {decryptWithPrivateKey} from "../cryptography/rsa.cryptography";
import {decryptAes256Gcm} from "../cryptography/aes-256-gcm.cryptography";

export class DecryptFileUseCase{
    constructor(
        private readonly fileService: FileService,
        private readonly keyService: KeyService,
    ) {}

    async execute(userId: string, fileId: string, password: string): Promise<Buffer> {
        const response: FileResponseDto = await this.fileService.getFileInfo(userId, fileId);
        const userFileAccessResponse = response.userFileAccess;
        const file = response.file;
        const privateKey: UserPrivateKeyDto = await this.keyService.getUserPrivateKey(userId);
        const decryptedPrivateKey = decryptPrivateKey(privateKey, password);
        const decryptedFEK = decryptWithPrivateKey(Buffer.from(userFileAccessResponse.encryptedFileKey, 'base64'), decryptedPrivateKey);
        const decryptedFile = decryptAes256Gcm({cipherText: file.fileContents, nonce: file.nonce, tag: file.tag}, decryptedFEK);
        console.log(decryptedFile.toString('utf8'));
        return decryptedFile;
    }
}