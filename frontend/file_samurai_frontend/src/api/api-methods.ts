import {AddFileDto} from "../models/addFileDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import axiosInstance from "./axios-instance";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {FileResponseDto} from "../models/fileResponseDto";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";

export async function postFile(dto: AddFileDto): Promise<AddFileResponseDto> {
    const response = await axiosInstance.post<AddFileResponseDto>(`file`, dto);
    return response.data;
}

export async function getUserPublicKey(userId: string): Promise<string> {
    const response = await axiosInstance.get<string>(`keypair/public/${userId}`, {
        headers: {
            'Accept': 'text/plain',
        },
    });
    return response.data;
}

export async function postUserFileAccess(dto: AddOrGetUserFileAccessDto): Promise<void> {
    await axiosInstance.post<AddOrGetUserFileAccessDto>('file/access', dto);
}

export async function getFileInfo(userId: string, fileId: string): Promise<FileResponseDto> {
    const response = await axiosInstance.get<FileResponseDto>(`file`, {
        params: { userId, fileId }
    });
    return response.data;
}

export async function getUserPrivateKey(userId: string): Promise<UserPrivateKeyDto> {
    const response = await axiosInstance.get<UserPrivateKeyDto>(`keypair/private/${userId}`);
    return response.data;
}

export async function getEncryptedFileKey(userId: string, fileId: string): Promise<string> {
    const response = await axiosInstance.get<AddOrGetUserFileAccessDto>(`file/access`, {
        params: { userId, fileId }
    });
    return response.data.encryptedFileKey;
}

export async function postKeypair(dto: AddUserKeyPairDto): Promise<void> {
    await axiosInstance.post<AddFileResponseDto>(`keypair`, dto);
}