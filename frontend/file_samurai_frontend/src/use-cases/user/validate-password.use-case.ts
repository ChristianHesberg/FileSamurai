import {UserService} from "../../services/user.service";

export class ValidatePasswordUseCase {

    constructor(private readonly userService: UserService) {
    }

    async execute(password: string) {
        return await this.userService.validatePassword(password)
    }
}