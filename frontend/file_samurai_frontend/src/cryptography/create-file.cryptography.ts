import {generateKey} from "./utils.cryptography";
import {encryptAes256Gcm} from "./aes-256-gcm.cryptography"
import {get, post} from "../api/api_methods";
import {AddFileDto} from "../models/addFileDto";
import {encryptWithPublicKey} from "./rsa.cryptography";
import {AddUserFileAccessDto} from "../models/addUserFileAccessDto";
import {EDITOR_ROLE} from "../../constants";
import {AddFileResponseDto} from "../models/addFileResponseDto";

export async function createFile(userId: string, groupId: string, file: Buffer, title: string){
    const key = generateKey();
    const encryptedFileResponse = encryptFile(file, key, title, groupId);
    const fileResponse: AddFileResponseDto = await postFile(encryptedFileResponse);

    const userPublicKey: string = await getUserPublicKey(userId);
    const encryptedFAK = encryptWithPublicKey(key, userPublicKey);
    const addUserFileAccessDto: AddUserFileAccessDto = generateUserFileAccessDto(encryptedFAK, userId, fileResponse.id);
    await post('file/access', addUserFileAccessDto);
}

const encryptFile = (file: Buffer, key: Buffer, title: string, groupId: string): AddFileDto => {
    const encryptedFile = encryptAes256Gcm(file, key);
    return {
        fileContents: encryptedFile.cipherText,
        nonce: encryptedFile.nonce,
        tag: encryptedFile.tag,
        title: title,
        groupId: groupId
    }
}

const generateUserFileAccessDto = (encryptedFAK: Buffer, userId: string, fileId: string): AddUserFileAccessDto => {
    return {
        encryptedFileKey: encryptedFAK.toString('base64'),
        role: EDITOR_ROLE,
        userId: userId,
        fileId: fileId
    }
}

async function postFile(dto: AddFileDto): Promise<AddFileResponseDto> {
    try {
        const fileResult = await post('file', dto);
        return await fileResult.json();
    } catch(error){
        console.log(error);
        throw error;
    }
}

async function getUserPublicKey(userId: string): Promise<string> {
    try {
        const userPublicKeyRequest = await get('keypair', `public/${userId}`)
        return await userPublicKeyRequest.text();
    } catch(error){
        console.log(error);
        throw error;
    }
}