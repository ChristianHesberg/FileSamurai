import {FileResponseDto} from "../models/fileResponseDto";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {decryptAes256Gcm} from "./aes-256-gcm.cryptography";
import {decryptPrivateKey} from "./utils.cryptography";
import {decryptWithPrivateKey} from "./rsa.cryptography";
import {getFileInfo, getUserPrivateKey} from "../api/api-methods";

export async function decryptFile(userId: string, fileId: string, password: string){
    const response: FileResponseDto = await getFileInfo(userId, fileId);
    const userFileAccessResponse = response.userFileAccess;
    const file = response.file;
    const privateKey: UserPrivateKeyDto = await getUserPrivateKey(userId);
    const decryptedPrivateKey = decryptPrivateKey(privateKey, password);
    const decryptedFEK = decryptWithPrivateKey(Buffer.from(userFileAccessResponse.encryptedFileKey, 'base64'), decryptedPrivateKey);
    const decryptedFile = decryptAes256Gcm({cipherText: file.fileContents, nonce: file.nonce, tag: file.tag}, decryptedFEK);
    console.log(decryptedFile.toString('utf8'));
}
