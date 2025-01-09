import {FileResponseDto} from "../../models/fileResponseDto";
import {UserPrivateKeyDto} from "../../models/userPrivateKeyDto";
import {ICryptographyService} from "../../services/cryptography.service.interface";
import { Buffer } from "buffer";
import {IFileService} from "../../services/file.service.interface";
import {IKeyService} from "../../services/key.service.interface";

export class DecryptFileUseCase{
    constructor(
        private readonly fileService: IFileService,
        private readonly keyService: IKeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(userId: string, fileId: string, cryptoKey: CryptoKey): Promise<Buffer> {
        const response: FileResponseDto = await this.fileService.getFileInfo(userId, fileId);
        const userFileAccessResponse = response.userFileAccess;
        const file = response.file;
        const privateKey: UserPrivateKeyDto = await this.keyService.getUserPrivateKey(userId);
        const decryptedPrivateKey = await this.cryptoService.decryptPrivateKey(privateKey, cryptoKey);
        console.log(decryptedPrivateKey);
        const decryptedFEK = await this.cryptoService.decryptWithPrivateKey(Buffer.from(userFileAccessResponse.encryptedFileKey, 'base64'), decryptedPrivateKey);
        const decryptedFile = await this.cryptoService.decryptAes256GcmWithBufferKey({cipherText: file.fileContents, nonce: file.nonce}, decryptedFEK);

        return decryptedFile;
    }
}