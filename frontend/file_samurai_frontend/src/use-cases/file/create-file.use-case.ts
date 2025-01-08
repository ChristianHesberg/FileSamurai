import {AddFileResponseDto} from "../../models/addFileResponseDto";
import {AddOrGetUserFileAccessDto} from "../../models/addOrGetUserFileAccessDto";
import {EDITOR_ROLE} from "../../constants";
import {AddFileDto} from "../../models/addFileDto";
import {IFileService} from "../../services/file.service.interface";
import {IKeyService} from "../../services/key.service.interface";
import {ICryptographyService} from "../../services/cryptography.service.interface";

export class CreateFileUseCase {
    constructor(
        private readonly fileService: IFileService,
        private readonly keyService: IKeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(userId: string, groupId: string, file: Buffer, title: string): Promise<AddFileResponseDto>{
        const key = await this.cryptoService.generateKey(32);
        const encryptedFileResponse = await this.encryptFile(file, key, title, groupId);
        const fileResponse: AddFileResponseDto = await this.fileService.postFile(encryptedFileResponse);

        const userPublicKey: string = await this.keyService.getUserPublicKey(userId);
        const encryptedFAK = await this.cryptoService.encryptWithPublicKey(key, userPublicKey);

        const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.convertToUserFileAccessDto(encryptedFAK, userId, fileResponse.id, EDITOR_ROLE);
        await this.fileService.postUserFileAccess(addUserFileAccessDto);
        return fileResponse;
    }

    async encryptFile(file: Buffer, key: Buffer, title: string, groupId: string): Promise<AddFileDto> {
        const encryptedFile = await this.cryptoService.encryptAes256Gcm(file, key);
        return {
            fileContents: encryptedFile.cipherText,
            nonce: encryptedFile.nonce,
            title: title,
            groupId: groupId
        }
    }
}