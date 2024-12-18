import {FileResponseDto} from "../models/fileResponseDto";
import axiosInstance from "../api/axios-instance";
import {AddFileDto} from "../models/addFileDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";

export async function getFileInfo(userId: string, fileId: string): Promise<FileResponseDto> {
    const response = await axiosInstance.get<FileResponseDto>(`file`, {
        params: { userId, fileId }
    });
    return response.data;
}

export async function postFile(dto: AddFileDto): Promise<AddFileResponseDto> {
    const response = await axiosInstance.post<AddFileResponseDto>(`file`, dto);
    return response.data;
}

export async function postUserFileAccess(dto: AddOrGetUserFileAccessDto): Promise<void> {
    await axiosInstance.post<AddOrGetUserFileAccessDto>('file/access', dto);
}

export const generateUserFileAccessDto = (encryptedFAK: Buffer, userId: string, fileId: string, role: string): AddOrGetUserFileAccessDto => {
    return {
        encryptedFileKey: encryptedFAK.toString('base64'),
        role: role,
        userId: userId,
        fileId: fileId
    }
}