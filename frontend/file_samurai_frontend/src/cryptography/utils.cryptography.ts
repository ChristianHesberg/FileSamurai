import {pbkdf2Sync, randomBytes} from "node:crypto";
import {decryptAes256Gcm, encryptAes256Gcm} from "./aes-256-gcm.cryptography";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {AddFileDto} from "../models/addFileDto";
import {AddOrGetUserFileAccessDto} from "../models/addOrGetUserFileAccessDto";
import {EDITOR_ROLE} from "../../constants";

export function deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Buffer {
    return pbkdf2Sync(password, salt, 100000, keyLength, 'sha512');
}

export function generateKey(size: number = 12): Buffer {
    return randomBytes(size);
}

export function decryptPrivateKey(privateKey: UserPrivateKeyDto, password: string): Buffer{
    const aesInput = {
        cipherText: privateKey.privateKey,
        nonce: privateKey.nonce,
        tag: privateKey.tag
    }
    return decryptAes256Gcm(
        aesInput,
        deriveKeyFromPassword(password, Buffer.from(privateKey.salt, 'base64'))
    );
}

export const generateUserFileAccessDto = (encryptedFAK: Buffer, userId: string, fileId: string, role: string): AddOrGetUserFileAccessDto => {
    return {
        encryptedFileKey: encryptedFAK.toString('base64'),
        role: role,
        userId: userId,
        fileId: fileId
    }
}