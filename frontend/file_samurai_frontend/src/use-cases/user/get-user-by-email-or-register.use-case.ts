import {UserService} from "../../services/user.service";

export class GetUserByEmailOrRegisterUseCase {
    constructor(private readonly userService: UserService) {}

    async execute(email: string) {
        return await this.userService.getUserIfNullRegister(email);
    }
}