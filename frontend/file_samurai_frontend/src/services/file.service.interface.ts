import {FileResponseDto} from "../models/fileResponseDto";
import {AddFileDto} from "../models/addFileDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";

export interface IFileService {
    getFileInfo(userId: string, fileId: string): Promise<FileResponseDto>;
    postFile(dto: AddFileDto): Promise<AddFileResponseDto>;
    postUserFileAccess(dto: AddOrGetUserFileAccessDto): Promise<void>;
    convertToUserFileAccessDto(encryptedFAK: Buffer, userId: string, fileId: string, role: string): AddOrGetUserFileAccessDto;
}