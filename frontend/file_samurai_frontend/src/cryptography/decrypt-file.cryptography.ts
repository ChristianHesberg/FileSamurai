import {API_BASE_URL} from "../../constants";
import {FileResponseDto} from "../models/fileResponseDto";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {decryptAes256Gcm} from "./aes-256-gcm.cryptography";
import {deriveKeyFromPassword} from "./utils.cryptography";
import {decryptWithPrivateKey} from "./rsa.cryptography";

export async function decryptFile(userId: string, fileId: string, password: string){
    const response: FileResponseDto = await getFileInfo(userId, fileId);
    const userFileAccessResponse = response.userFileAccess;
    const file = response.file;
    const obj: UserPrivateKeyDto = await getUserPrivateKey(userId);
    const decryptedPrivateKey = decryptAes256Gcm({cipherText: obj.privateKey, nonce: obj.nonce, tag: obj.tag}, deriveKeyFromPassword(password, Buffer.from(obj.salt, 'base64')));
    const decryptedFEK = decryptWithPrivateKey(Buffer.from(userFileAccessResponse.encryptedFileKey, 'base64'), decryptedPrivateKey);
    const decryptedFile = decryptAes256Gcm({cipherText: file.fileContents, nonce: file.nonce, tag: file.tag}, decryptedFEK);
    console.log(decryptedFile.toString('utf8'));
}

//TODO surely we will most definitely fix this later
async function getFileInfo(userId: string, fileId: string): Promise<FileResponseDto> {
    try {
        const response = await fetch(`${API_BASE_URL}/file?userId=${userId}&fileId=${fileId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                //add jwt here
            }
        })
        return await response.json();
    } catch(error){
        console.log(error);
        throw error;
    }
}

async function getUserPrivateKey(userId: string): Promise<UserPrivateKeyDto> {
    try {
        const response = await fetch(`${API_BASE_URL}/keypair/private/${userId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                //add jwt here
            }
        })
        return await response.json();
    } catch(error){
        console.log(error);
        throw error;
    }
}

decryptFile('e9171352-c0e3-4705-8f52-5afca618c8b2', '32b1dd50-fee9-40a7-aedd-820d542f5bfa', 'cool@email.com-very_secret_password_that_you_cannot_guess');
