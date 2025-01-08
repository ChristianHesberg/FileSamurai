import {UserService} from "../../services/user.service";

export class GetUserByTokenUseCase {
    constructor(private readonly userService:UserService) {
    }

    async execute(){
        return await this.userService.getUserByToken();
    }
}