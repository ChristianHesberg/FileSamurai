import {KeyService} from "../services/key.service";
import {EncryptedRsaKeyPairModel} from "../models/encryptedRsaKeyPair.model";
import {AddUserKeyPairDto} from "../models/addUserKeyPairDto";
import {CryptographyService} from "../services/cryptography.service";

export class CreateUserKeyPair {
    constructor(
        private readonly keyService: KeyService,
        private readonly cryptoService: CryptographyService,
    ) {}

    async execute(password: string, email: string, userId: string): Promise<void> {
        const concatenatedPassword: string = `${email}-${password}`;
        const keyPair: EncryptedRsaKeyPairModel = this.cryptoService.generateRsaKeyPairWithEncryption(concatenatedPassword);

        const dto: AddUserKeyPairDto = {
            userId: userId,
            publicKey: keyPair.publicKey,
            privateKey: keyPair.privateKey,
            nonce: keyPair.nonce,
            tag: keyPair.tag,
            salt: keyPair.salt
        };
        await this.keyService.postKeypair(dto);
    }
}