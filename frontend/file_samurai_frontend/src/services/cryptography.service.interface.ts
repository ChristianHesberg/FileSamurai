import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";

export interface CryptographyServiceInterface {
    encryptAes256Gcm(plaintext: Buffer, key: Buffer): AesGcmEncryptionOutput;
    decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: Buffer): Buffer;
    generateRsaKeyPair(): RsaKeyPairModel;
    generateRsaKeyPairWithEncryption(password: string, saltSize: number): EncryptedRsaKeyPairModel;
    encryptWithPublicKey(data: Buffer, publicKey: string): Buffer;
    decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Buffer;
    deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number): Buffer;
    generateKey(size: number): Buffer;
    decryptPrivateKey(privateKey: UserPrivateKeyDto, password: string): Buffer;
}