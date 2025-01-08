import {CryptographyService} from "../../services/cryptography.service";

export class ValidatePasswordHashUseCase {
    constructor(private readonly cryptographyService: CryptographyService) {
    }

    async execute(password: string, salt: string, hashedPassword: string): Promise<boolean> {
        const newHash = await this.cryptographyService.hashPassword(password, Buffer.from(salt, "base64"))

        return newHash.toString("base64") === hashedPassword;
    }
}