import type {FileService} from "../services/file.service";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import type {KeyService} from "../services/key.service";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {EDITOR_ROLE} from "../constants";
import {AddFileDto} from "../models/addFileDto";
import {CryptographyService} from "../services/cryptography.service";

export class CreateFileUseCase {
    constructor(
        private readonly fileService: FileService,
        private readonly keyService: KeyService,
        private readonly cryptoService: CryptographyService,
    ) {}

    async execute(userId: string, groupId: string, file: Buffer, title: string): Promise<AddFileResponseDto>{
        const key = this.cryptoService.generateKey(32);
        const encryptedFileResponse = this.encryptFile(file, key, title, groupId);
        const fileResponse: AddFileResponseDto = await this.fileService.postFile(encryptedFileResponse);

        const userPublicKey: string = await this.keyService.getUserPublicKey(userId);
        const encryptedFAK = this.cryptoService.encryptWithPublicKey(key, userPublicKey);

        const addUserFileAccessDto: AddOrGetUserFileAccessDto = this.fileService.generateUserFileAccessDto(encryptedFAK, userId, fileResponse.id, EDITOR_ROLE);
        await this.fileService.postUserFileAccess(addUserFileAccessDto);
        return fileResponse;
    }

    encryptFile = (file: Buffer, key: Buffer, title: string, groupId: string): AddFileDto => {
        const encryptedFile = this.cryptoService.encryptAes256Gcm(file, key);
        return {
            fileContents: encryptedFile.cipherText,
            nonce: encryptedFile.nonce,
            tag: encryptedFile.tag,
            title: title,
            groupId: groupId
        }
    }
}