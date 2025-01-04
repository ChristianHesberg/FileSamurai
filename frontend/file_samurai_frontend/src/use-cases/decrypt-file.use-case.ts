import type {FileService} from "../services/file.service";
import {FileResponseDto} from "../models/fileResponseDto";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import type {KeyService} from "../services/key.service";
import {CryptographyService} from "../services/cryptography.service";
import {CryptographyServiceInterface} from "../services/cryptography.service.interface";

export class DecryptFileUseCase{
    constructor(
        private readonly fileService: FileService,
        private readonly keyService: KeyService,
        private readonly cryptoService: CryptographyServiceInterface,
    ) {}

    async execute(userId: string, fileId: string, password: string): Promise<Buffer> {
        const response: FileResponseDto = await this.fileService.getFileInfo(userId, fileId);
        const userFileAccessResponse = response.userFileAccess;
        const file = response.file;
        const privateKey: UserPrivateKeyDto = await this.keyService.getUserPrivateKey(userId);
        const decryptedPrivateKey = await this.cryptoService.decryptPrivateKey(privateKey, password);
        const decryptedFEK = await this.cryptoService.decryptWithPrivateKey(Buffer.from(userFileAccessResponse.encryptedFileKey, 'base64'), decryptedPrivateKey);
        const decryptedFile = await this.cryptoService.decryptAes256Gcm({cipherText: file.fileContents, nonce: file.nonce}, decryptedFEK);
        console.log(decryptedFile.toString('utf8'));
        return decryptedFile;
    }
}