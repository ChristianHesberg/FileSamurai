import axiosInstance from "../api/axios-instance";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {IKeyService} from "./key.service.interface";

export class KeyService implements IKeyService{
    async getUserPublicKey(userId: string): Promise<string> {
        const response = await axiosInstance.get<string>(`keypair/public/${userId}`, {
            headers: {
                'Accept': 'text/plain',
            },
        });
        return response.data;
    }

    async getUserPublicKeys(ids: string[]): Promise<string[]> {
        const idsAsString = ids.join(',');
        const response = await axiosInstance.get<string[]>(`keypair/public?ids=${idsAsString}`);
        return response.data;
    }

    async getUserPrivateKey(userId: string): Promise<UserPrivateKeyDto> {
        const response = await axiosInstance.get<UserPrivateKeyDto>(`keypair/private/${userId}`);
        return response.data;
    }

    async getEncryptedFileKey(userId: string, fileId: string): Promise<string> {
        const response = await axiosInstance.get<AddOrGetUserFileAccessDto>(`file/access`, {
            params: { userId, fileId }
        });
        return response.data.encryptedFileKey;
    }

    async postKeypair(dto: AddUserKeyPairDto): Promise<void> {
        await axiosInstance.post<AddFileResponseDto>(`keypair`, dto);
    }
}