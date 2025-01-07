import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {ICryptographyService} from "../services/cryptography.service.interface";
import {Buffer} from 'buffer';

export class ShareFileUseCase{
    constructor(
        private readonly fileService: FileService,
        private readonly keyService: KeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(ownerId: string, recipientId: string, fileId: string, ownerPassword: string, role: string): Promise<void>{
        const encryptedFileKey = await this.keyService.getEncryptedFileKey(ownerId, fileId);
        const privateKey = await this.keyService.getUserPrivateKey(ownerId);

        const decryptedPrivateKey = await this.cryptoService.decryptPrivateKey(privateKey, ownerPassword);
        const decryptedFEK = await this.cryptoService.decryptWithPrivateKey(Buffer.from(encryptedFileKey, 'base64'), decryptedPrivateKey);

        const shareePublicKey = await this.keyService.getUserPublicKey(recipientId);
        const encryptedFAK = await this.cryptoService.encryptWithPublicKey(decryptedFEK, shareePublicKey);

        const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.convertToUserFileAccessDto(encryptedFAK, recipientId, fileId, role);
        await this.fileService.postUserFileAccess(addUserFileAccessDto);
    }
}