import {FileResponseDto} from "../models/fileResponseDto";
import axiosInstance from "../api/axios-instance";
import {AddFileDto} from "../models/addFileDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {FileServiceInterface} from "./file.service.interface";

export class FileService implements FileServiceInterface{
     async getFileInfo(userId: string, fileId: string): Promise<FileResponseDto> {
        const response = await axiosInstance.get<FileResponseDto>(`file`, {
            params: { userId, fileId }
        });
        return response.data;
    }

    async postFile(dto: AddFileDto): Promise<AddFileResponseDto> {
        const response = await axiosInstance.post<AddFileResponseDto>(`file`, dto);
        return response.data;
    }

    async postUserFileAccess(dto: AddOrGetUserFileAccessDto): Promise<void> {
        await axiosInstance.post<AddOrGetUserFileAccessDto>('file/access', dto);
    }

    convertToUserFileAccessDto(encryptedFAK: Buffer, userId: string, fileId: string, role: string): AddOrGetUserFileAccessDto {
        return {
            encryptedFileKey: encryptedFAK.toString('base64'),
            role: role,
            userId: userId,
            fileId: fileId
        }
    }
}



