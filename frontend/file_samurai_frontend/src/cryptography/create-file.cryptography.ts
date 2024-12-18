import {generateKey} from "./utils.cryptography";
import {encryptAes256Gcm} from "./aes-256-gcm.cryptography"
import {AddFileDto} from "../models/addFileDto";
import {encryptWithPublicKey} from "./rsa.cryptography";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {AddFileResponseDto} from "../models/addFileResponseDto";
import {generateUserFileAccessDto, postFile, postUserFileAccess} from "../services/file.service";
import {getUserPublicKey} from "../services/key.service";
import {EDITOR_ROLE} from "../../constants";

export async function createFile(userId: string, groupId: string, file: Buffer, title: string){
    const key = generateKey(32);
    const encryptedFileResponse = encryptFile(file, key, title, groupId);
    const fileResponse: AddFileResponseDto = await postFile(encryptedFileResponse);

    const userPublicKey: string = await getUserPublicKey(userId);
    const encryptedFAK = encryptWithPublicKey(key, userPublicKey);

    const addUserFileAccessDto: AddOrGetUserFileAccessDto = generateUserFileAccessDto(encryptedFAK, userId, fileResponse.id, EDITOR_ROLE);
    await postUserFileAccess(addUserFileAccessDto);
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






