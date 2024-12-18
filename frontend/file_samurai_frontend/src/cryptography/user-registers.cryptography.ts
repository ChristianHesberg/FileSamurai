import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {generateRsaKeyPairWithEncryption} from "./rsa.cryptography";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import axiosInstance from "../api/axios-instance";

export async function createUserKeyPair(password: string, email: string, userId: string){
    const concatenatedPassword: string = `${email}-${password}`;
    const keyPair: EncryptedRsaKeyPairModel = generateRsaKeyPairWithEncryption(concatenatedPassword);

    const dto: AddUserKeyPairDto = {
        userId: userId,
        publicKey: keyPair.publicKey,
        privateKey: keyPair.privateKey,
        nonce: keyPair.nonce,
        tag: keyPair.tag,
        salt: keyPair.salt
    };
    await postKeypair(dto);
}

async function postKeypair(dto: AddUserKeyPairDto): Promise<void> {
    await axiosInstance.post<AddFileResponseDto>(`keypair`, dto);
}
