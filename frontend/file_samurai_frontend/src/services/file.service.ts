import {FileResponseDto} from "../models/fileResponseDto";
import axiosInstance from "../api/axios-instance";
import {AddFileDto} from "../models/addFileDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {IFileService} from "./file.service.interface";
import {FileDto} from "../models/FileDto";
import {SelectionOption} from "../models/selectionOption";
import {FileOption} from "../models/FileOption";
import {FileAccessDto} from "../models/FileAccessDto";


export class FileService implements IFileService {
    async getFileInfo(userId: string, fileId: string): Promise<FileResponseDto> {
        const response = await axiosInstance.get<FileResponseDto>(`file`, {
            params: {userId, fileId}
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

    async postUserFileAccesses(dtos: AddOrGetUserFileAccessDto[]): Promise<void> {
        await axiosInstance.post<AddOrGetUserFileAccessDto[]>('file/accesses', dtos);
    }

    convertToUserFileAccessDto(encryptedFAK: Buffer, userId: string, fileId: string, role: string): AddOrGetUserFileAccessDto {
        return {
            encryptedFileKey: encryptedFAK.toString('base64'),
            role: role,
            userId: userId,
            fileId: fileId
        }
    }

    async getFileOptions(userId: string): Promise<FileOption[]> {
        const response = await axiosInstance.get<FileOption[]>(`file/fileOptions/${userId}`)
        return response.data
    }

    async getAllFileAccess(fileId: string): Promise<FileAccessDto[]> {
        const response = await axiosInstance.get<FileAccessDto[]>(`file/allFileAccess/${fileId}`)
        return response.data
    }

    async deleteUserFileAccess(userId: string, fileId: string): Promise<void> {
        const response = await axiosInstance.delete(`file/access?fileId=${fileId}&userId=${userId}`)
        return response.data
    }

    async deleteFile(fileId: string) {
        const response = await axiosInstance.delete(`file/${fileId}`)
    }
}



