import {AesGcmEncryptionOutput} from "../models/aesGcmEncryptionOutput.model";
import {RsaKeyPairModel} from "../models/rsaKeyPair.model";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {UserPrivateKeyDto} from "../models/userPrivateKeyDto";
import {Buffer} from "buffer";

export interface ICryptographyService {
    encryptAes256Gcm(plaintext: Buffer, key: CryptoKey): Promise<AesGcmEncryptionOutput>;
    encryptAes256GcmWithBufferKey(plaintext: Buffer, key: Buffer): Promise<AesGcmEncryptionOutput>;
    decryptAes256Gcm(encryptedData: AesGcmEncryptionOutput, key: CryptoKey): Promise<Buffer>;
    decryptAes256GcmWithBufferKey(encryptedData: AesGcmEncryptionOutput, key: Buffer): Promise<Buffer>;
    generateRsaKeyPair(): Promise<RsaKeyPairModel>;
    generateRsaKeyPairWithEncryption(key: CryptoKey, salt: Buffer): Promise<EncryptedRsaKeyPairModel>;
    encryptWithPublicKey(data: Buffer, publicKey: string): Promise<Buffer>;
    decryptWithPrivateKey(encryptedData: Buffer, privateKey: Buffer): Promise<Buffer>;
    deriveKeyFromPassword(password: string, salt: Buffer, keyLength: number): Promise<CryptoKey>;
    generateKey(size: number): Promise<Buffer>;
    decryptPrivateKey(privateKey: UserPrivateKeyDto, cryptoKey: CryptoKey): Promise<Buffer>;
    hashPassword(password: string, salt: Buffer): Promise<Buffer>
}