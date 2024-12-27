import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {decryptPrivateKey} from "../cryptography/utils.cryptography";
import {decryptWithPrivateKey, encryptWithPublicKey} from "../cryptography/rsa.cryptography";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";

export class ShareFileUseCase{
    constructor(
        private readonly fileService: FileService,
        private readonly keyService: KeyService
    ) {}

    async execute(ownerId: string, recipientId: string, fileId: string, ownerPassword: string, role: string): Promise<void>{
        const encryptedFileKey = await this.keyService.getEncryptedFileKey(ownerId, fileId);
        const privateKey = await this.keyService.getUserPrivateKey(ownerId);

        const decryptedPrivateKey = decryptPrivateKey(privateKey, ownerPassword)
        const decryptedFEK = decryptWithPrivateKey(Buffer.from(encryptedFileKey, 'base64'), decryptedPrivateKey);

        const shareePublicKey = await this.keyService.getUserPublicKey(recipientId);
        const encryptedFAK = encryptWithPublicKey(decryptedFEK, shareePublicKey);

        const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.generateUserFileAccessDto(encryptedFAK, recipientId, fileId, role);
        await this.fileService.postUserFileAccess(addUserFileAccessDto);
    }
}