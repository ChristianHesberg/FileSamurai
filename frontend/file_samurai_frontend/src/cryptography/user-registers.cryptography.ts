import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {generateRsaKeyPairWithEncryption} from "./rsa.cryptography";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {postKeypair} from "../services/key.service";

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
