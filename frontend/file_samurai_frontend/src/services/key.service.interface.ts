import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {RsaPublicKeyWithIdModel} from "../models/rsaPublicKeyWithId.model";

export interface IKeyService {
    getUserPublicKey(userId: string): Promise<string>;
    getUserPublicKeys(ids: string[]): Promise<RsaPublicKeyWithIdModel[]>;
    getUserPrivateKey(userId: string): Promise<UserPrivateKeyDto>;
    getEncryptedFileKey(userId: string, fileId: string): Promise<string>;
    postKeypair(dto: AddUserKeyPairDto): Promise<void>;
}