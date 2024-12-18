import {decryptPrivateKey, generateUserFileAccessDto} from "./utils.cryptography";
import {decryptWithPrivateKey, encryptWithPublicKey} from "./rsa.cryptography";
import {getEncryptedFileKey, getUserPrivateKey, getUserPublicKey, postUserFileAccess} from "../api/api-methods";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {VIEWER_ROLE} from "../../constants";

export async function shareFile(ownerId: string, recipientId: string, fileId: string, ownerPassword: string, role: string){
    const encryptedFileKey = await getEncryptedFileKey(ownerId, fileId);
    const privateKey = await getUserPrivateKey(ownerId);

    const decryptedPrivateKey = decryptPrivateKey(privateKey, ownerPassword)
    const decryptedFEK = decryptWithPrivateKey(Buffer.from(encryptedFileKey, 'base64'), decryptedPrivateKey);

    const shareePublicKey = await getUserPublicKey(recipientId);
    const encryptedFAK = encryptWithPublicKey(decryptedFEK, shareePublicKey);

    const addUserFileAccessDto: AddOrGetUserFileAccessDto = generateUserFileAccessDto(encryptedFAK, recipientId, fileId, role);
    await postUserFileAccess(addUserFileAccessDto);
}



