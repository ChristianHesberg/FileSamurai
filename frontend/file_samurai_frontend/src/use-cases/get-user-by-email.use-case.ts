import {UserService} from "../services/user.service";

export class GetUserByEmailUseCase{
    constructor(private readonly userService:UserService) {
    }

    async execute(email:string){
        return await this.userService.getUserByEmail(email);
    }
}