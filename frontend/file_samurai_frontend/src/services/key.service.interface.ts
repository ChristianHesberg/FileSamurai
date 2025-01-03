import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";

export interface KeyServiceInterface {
    getUserPublicKey(userId: string): Promise<string>;
    getUserPrivateKey(userId: string): Promise<UserPrivateKeyDto>;
    getEncryptedFileKey(userId: string, fileId: string): Promise<string>;
    postKeypair(dto: AddUserKeyPairDto): Promise<void>;
}