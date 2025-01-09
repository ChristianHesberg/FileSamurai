import {AddOrGetUserFileAccessDto} from "../../models/addOrGetUserFileAccessDto";
import {ICryptographyService} from "../../services/cryptography.service.interface";
import {Buffer} from 'buffer';
import {IFileService} from "../../services/file.service.interface";
import {IKeyService} from "../../services/key.service.interface";

export class ShareFileWithMultipleUsersUseCase{
    constructor(
        private readonly fileService: IFileService,
        private readonly keyService: IKeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(ownerId: string, recipientIds: string[], fileId: string, ownerCryptoKey: CryptoKey, role: string): Promise<void>{
        const encryptedFileKey = await this.keyService.getEncryptedFileKey(ownerId, fileId);
        const privateKey = await this.keyService.getUserPrivateKey(ownerId);

        const decryptedPrivateKey = await this.cryptoService.decryptPrivateKey(privateKey, ownerCryptoKey);
        const decryptedFEK = await this.cryptoService.decryptWithPrivateKey(Buffer.from(encryptedFileKey, 'base64'), decryptedPrivateKey);

        const recipientPublicKeys = await this.keyService.getUserPublicKeys(recipientIds);
        console.log(recipientPublicKeys)

        const accesses: AddOrGetUserFileAccessDto[] = [];
        for(const key of recipientPublicKeys) {
            const encryptedFAK = await this.cryptoService.encryptWithPublicKey(decryptedFEK, key.publicKey);
            const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.convertToUserFileAccessDto(encryptedFAK, key.userId, fileId, role);
            accesses.push(addUserFileAccessDto);
        }

        await this.fileService.postUserFileAccesses(accesses);
    }
}