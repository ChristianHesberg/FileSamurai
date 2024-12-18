import axiosInstance from "../api/axios-instance";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";

export async function getUserPublicKey(userId: string): Promise<string> {
    const response = await axiosInstance.get<string>(`keypair/public/${userId}`, {
        headers: {
            'Accept': 'text/plain',
        },
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