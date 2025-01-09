import {AddOrGetUserFileAccessDto} from "../../models/addOrGetUserFileAccessDto";
import {ICryptographyService} from "../../services/cryptography.service.interface";
import {Buffer} from 'buffer';
import {IFileService} from "../../services/file.service.interface";
import {IKeyService} from "../../services/key.service.interface";

export class ShareFileUseCase{
    constructor(
        private readonly fileService: IFileService,
        private readonly keyService: IKeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(ownerId: string, recipientId: string, fileId: string, ownerCryptoKey: CryptoKey, role: string): Promise<void>{
        const encryptedFileKey = await this.keyService.getEncryptedFileKey(ownerId, fileId);
        const privateKey = await this.keyService.getUserPrivateKey(ownerId);

        const decryptedPrivateKey = await this.cryptoService.decryptPrivateKey(privateKey, ownerCryptoKey);
        const decryptedFEK = await this.cryptoService.decryptWithPrivateKey(Buffer.from(encryptedFileKey, 'base64'), decryptedPrivateKey);

        const shareePublicKey = await this.keyService.getUserPublicKey(recipientId);
        const encryptedFAK = await this.cryptoService.encryptWithPublicKey(decryptedFEK, shareePublicKey);

        const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.convertToUserFileAccessDto(encryptedFAK, recipientId, fileId, role);
        await this.fileService.postUserFileAccess(addUserFileAccessDto);
    }
}