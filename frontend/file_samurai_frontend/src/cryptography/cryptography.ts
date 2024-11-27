import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {createCipheriv, createDecipheriv, generateKeyPairSync, pbkdf2Sync, randomBytes} from "node:crypto";
import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";

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


function deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number = 32): Buffer {
    return pbkdf2Sync(password, salt, 100000, keyLength, 'sha512');
}

function encrypt(plaintext: string, password: string, saltSize: number = 16): AesGcmEncryptionOutput {
    const salt = randomBytes(saltSize);
    const key = deriveKeyFromPassword(password, salt);

    const nonce = randomBytes(12);
    const cipher = createCipheriv('aes-256-gcm', key, nonce);

    const cipherTextBuffer = Buffer.concat([
        cipher.update(plaintext, 'utf8'),
        cipher.final()
    ]);
    const tag = cipher.getAuthTag();

    return {
        cipherText: cipherTextBuffer.toString('hex'),
        nonce: nonce.toString('hex'),
        tag: tag.toString('hex'),
        salt: salt.toString('hex')
    };
}

function decrypt(encryptedData: AesGcmEncryptionOutput, password: string): string {
    const { cipherText, nonce, tag, salt } = encryptedData;

    const key = deriveKeyFromPassword(password, Buffer.from(salt, 'hex'));

    const decipher = createDecipheriv('aes-256-gcm', key, Buffer.from(nonce, 'hex'));
    decipher.setAuthTag(Buffer.from(tag, 'hex'));

    const decryptedTextBuffer = Buffer.concat([
        decipher.update(Buffer.from(cipherText, 'hex')),
        decipher.final()
    ]);

    return decryptedTextBuffer.toString('utf8');
}

// Example usage
const rsaKeyPair = generateRsaKeyPair();
const password = 'securepassword';

console.log('unencrypted private key:', rsaKeyPair.private_key);
// Encrypt the private key
const encryptedPrivateKey = encrypt(rsaKeyPair.private_key, password);
console.log('Encrypted Private Key:', encryptedPrivateKey.cipherText);
console.log('Nonce:', encryptedPrivateKey.nonce);
console.log('Tag:', encryptedPrivateKey.tag);
console.log('Salt:', encryptedPrivateKey.salt);

// Decrypt the private key
const decryptedPrivateKey = decrypt(encryptedPrivateKey, password);
console.log('Decrypted Private Key:', decryptedPrivateKey);

// The public key is usually not encrypted, but you can if needed
console.log('Public Key:', rsaKeyPair.public_key);