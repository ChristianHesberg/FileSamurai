import {KeyService} from "../../services/key.service";
import {EncryptedRsaKeyPairModel} from "../../models/encryptedRsaKeyPair.model";
import {AddUserKeyPairDto} from "../../models/addUserKeyPairDto";
import {ICryptographyService} from "../../services/cryptography.service.interface";

export class CreateUserKeyPairUseCase {
    constructor(
        private readonly keyService: KeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(password: string, email: string, userId: string): Promise<void> {
        const concatenatedPassword: string = `${email}-${password}`;
        const keyPair: EncryptedRsaKeyPairModel = await this.cryptoService.generateRsaKeyPairWithEncryption(concatenatedPassword);

        const dto: AddUserKeyPairDto = {
            userId: userId,
            publicKey: keyPair.publicKey,
            privateKey: keyPair.privateKey,
            nonce: keyPair.nonce,
            salt: keyPair.salt
        };
        await this.keyService.postKeypair(dto);
    }
}