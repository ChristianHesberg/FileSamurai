import {CryptographyService} from "../../services/cryptography.service";

export class GeneratePasswordHashUseCase {
    constructor(private readonly cryptographyService: CryptographyService) {
    }

    async execute(password: string, email: string): Promise<{ hashedPassword: string, salt: string }> {
        const salt = await this.cryptographyService.generateKey(16);

        const hash = await this.cryptographyService.hashPassword(password, email, salt)

        return {hashedPassword: hash.toString("base64"), salt: salt.toString("base64")}
    }
}