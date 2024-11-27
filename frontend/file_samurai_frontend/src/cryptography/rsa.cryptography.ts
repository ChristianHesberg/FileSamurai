import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {generateKeyPairSync, privateDecrypt, publicEncrypt, randomBytes} from "node:crypto";
import { encryptAes256Gcm } from "./aes-256-gcm.cryptography"
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {deriveKeyFromPassword} from "./utils.cryptography";

export function generateRsaKeyPair(): RsaKeyPairModel {
    const { publicKey, privateKey } = generateKeyPairSync('rsa', {
        modulusLength: 2048,
        publicKeyEncoding: {
            type: 'spki',
            format: 'pem'
        },
        privateKeyEncoding: {
            type: 'pkcs8',
            format: 'der'
        }
    });

    return {
        private_key: privateKey,
        public_key: publicKey
    };
}

export function generateRsaKeyPairWithEncryption(password: string, saltSize: number = 16): EncryptedRsaKeyPairModel {
    const salt = randomBytes(saltSize);
    const key = deriveKeyFromPassword(password, salt);
    const { private_key, public_key } = generateRsaKeyPair();
    const { cipherText, nonce, tag } = encryptAes256Gcm(private_key, key);
    return {
        privateKey: cipherText,
        publicKey: public_key,
        nonce: nonce,
        tag: tag,
        salt: salt.toString('base64')
    }
}

function encryptFEKwithPublicKey(fek: Buffer, publicKey: string): Buffer {
    return publicEncrypt(publicKey, fek);
}

function decryptFEKwithPrivateKey(encryptedFek: Buffer, privateKey: Buffer): Buffer {
    return privateDecrypt(privateKey, encryptedFek);
}

//let obj = generateRsaKeyPairWithEncryption('secret');

/*console.log(JSON.stringify({
    id: 'id',
    privateKey: obj.privateKey,
    publicKey: obj.publicKey,
    nonce: obj.nonce,
    tag: obj.tag,
    salt: obj.salt,
}));*/
