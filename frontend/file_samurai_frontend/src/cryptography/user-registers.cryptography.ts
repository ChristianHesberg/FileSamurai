import {generateKey} from "./utils.cryptography";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {generateRsaKeyPairWithEncryption} from "./rsa.cryptography";
import {post} from "../api/api_methods";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";

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
    await post('keypair', dto);
}

createUserKeyPair('very_secret_password_that_you_cannot_guess', 'cool@email.com', 'e9171352-c0e3-4705-8f52-5afca618c8b2');