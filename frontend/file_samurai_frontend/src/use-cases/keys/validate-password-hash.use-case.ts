import {UserService} from "../../services/user.service";
import {PasswordHash} from "../../models/PasswordHash";
import {ICryptographyService} from "../../services/cryptography.service.interface";
import {CryptographyService} from "../../services/cryptography.service";
import {Buffer} from "buffer";
export class ValidatePasswordHashUseCase {
    constructor(private readonly cryptographyService: ICryptographyService, private readonly userService: UserService) {
    }

    async execute(password: string, email: string): Promise<boolean> {
        const hash: PasswordHash = await this.userService.getPasswordHash()
        const newHash = await this.cryptographyService.hashPassword(password, email, Buffer.from(hash.salt, "base64"))

        return newHash.toString("base64") === hash.passwordHash;
    }
}