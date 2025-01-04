import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";

export interface ICryptographyService {
    encryptAes256Gcm(plaintext: Buffer, key: Buffer): Promise<AesGcmEncryptionOutput>;
    decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: Buffer): Promise<Buffer>;
    generateRsaKeyPair(): Promise<RsaKeyPairModel>;
    generateRsaKeyPairWithEncryption(password: string): Promise<EncryptedRsaKeyPairModel>;
    encryptWithPublicKey(data: Buffer, publicKey: string): Promise<Buffer>;
    decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Promise<Buffer>;
    deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number): Promise<Buffer>;
    generateKey(size: number): Promise<Buffer>;
    decryptPrivateKey(privateKey: UserPrivateKeyDto, password: string): Promise<Buffer>;
}