import {EncryptedRsaKeyPairModel} from "../../models/encryptedRsaKeyPair.model";
import {AddUserKeyPairDto} from "../../models/addUserKeyPairDto";
import {ICryptographyService} from "../../services/cryptography.service.interface";
import {IKeyService} from "../../services/key.service.interface";

export class CreateUserKeyPairUseCase {
    constructor(
        private readonly keyService: IKeyService,
        private readonly cryptoService: ICryptographyService,
    ) {}

    async execute(password: string, email: string, userId: string): Promise<void> {
        const salt = await this.cryptoService.generateKey(12);
        const key = await this.cryptoService.deriveKeyFromPassword(password, email, salt, 32);

        const keyPair: EncryptedRsaKeyPairModel = await this.cryptoService.generateRsaKeyPairWithEncryption(key, salt);

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