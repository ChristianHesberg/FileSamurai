import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";

export class RegisterUserUseCase {
    constructor(private readonly userService: UserService) {
    }

    async execute(email:string,hashedPassword:string,salt:string): Promise<User>{
        return await this.userService.registerUser(email,hashedPassword,salt)
    }
}