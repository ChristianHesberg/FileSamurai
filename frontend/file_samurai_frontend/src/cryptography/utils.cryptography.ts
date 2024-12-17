import {pbkdf2Sync, randomBytes} from "node:crypto";

export function deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Buffer {
    return pbkdf2Sync(password, salt, 100000, keyLength, 'sha512');
}

export function generateKey(size: number = 12): Buffer {
    return randomBytes(size);
}