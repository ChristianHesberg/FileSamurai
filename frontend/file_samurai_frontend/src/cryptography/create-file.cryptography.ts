import { generateKey} from "./utils.cryptography";
import { encryptAes256Gcm } from "./aes-256-gcm.cryptography"
import {post, get} from "../api/api_methods";
import {AddFileDto} from "../models/addFileDto";

export async function createFile(userId: string, groupId: string, file: Buffer, title: string){
    const key = generateKey();
    const encryptedFile = encryptAes256Gcm(file, key);
    const fileDto: AddFileDto = {
        fileContents: encryptedFile.cipherText,
        nonce: encryptedFile.nonce,
        tag: encryptedFile.tag,
        title: title,
        groupId: groupId
    }
    await post('file', fileDto).catch(error => {
        console.log(error)
        return undefined;
    });
    await get('keypair', `public/${userId}`)

}