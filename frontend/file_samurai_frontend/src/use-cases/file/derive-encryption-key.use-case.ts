import {ICryptographyService} from "../../services/cryptography.service.interface";
import {IKeyService} from "../../services/key.service.interface";
import {Buffer} from 'buffer';

export class DeriveEncryptionKeyUseCase {
    constructor(
        private readonly cryptoService: ICryptographyService,
        private readonly keyService: IKeyService
    ) {}

    async execute(password: string, userId: string): Promise<CryptoKey> {
        const { salt } = await this.keyService.getUserPrivateKey(userId);

        return this.cryptoService.deriveKeyFromPassword(password, Buffer.from(salt, 'base64'), 32);
    }
}