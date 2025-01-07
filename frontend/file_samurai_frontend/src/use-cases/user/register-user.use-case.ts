import {UserService} from "../../services/user.service";

export class RegisterUserUseCase {
    constructor(private readonly userService: UserService) {
    }

    async execute(email:string,password:string){
        return await this.userService.registerUser(email,password)
    }
}