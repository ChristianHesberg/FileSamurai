import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {generateKeyPairSync, privateDecrypt, publicEncrypt, randomBytes} from "node:crypto";
import { encryptAes256Gcm, decryptAes256Gcm } from "./aes-256-gcm.cryptography"
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
            format: 'pem'
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
    const { cipherText, nonce, tag } = encryptAes256Gcm(Buffer.from(private_key), key);
    return {
        privateKey: cipherText,
        publicKey: public_key,
        nonce: nonce,
        tag: tag,
        salt: salt.toString('base64')
    }
}

function encryptWithPublicKey(data: Buffer, publicKey: string): Buffer {
    return publicEncrypt(publicKey, data);
}

function decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Buffer {
    return privateDecrypt(privateKey, encryptedData);
}

//let obj = generateRsaKeyPairWithEncryption('secret');

/*const test = encryptWithPublicKey(Buffer.from('some data'), obj.publicKey);
const decryptedPrivateKey = decryptAes256Gcm({cipherText: obj.privateKey, nonce: obj.nonce, tag: obj.tag}, deriveKeyFromPassword('secret', Buffer.from(obj.salt, 'base64')));
console.log(decryptWithPrivateKey(test, decryptedPrivateKey).toString());*/

/*console.log(JSON.stringify({
    id: 'id',
    privateKey: obj.privateKey,
    publicKey: obj.publicKey,
    nonce: obj.nonce,
    tag: obj.tag,
    salt: obj.salt,
}));*/
