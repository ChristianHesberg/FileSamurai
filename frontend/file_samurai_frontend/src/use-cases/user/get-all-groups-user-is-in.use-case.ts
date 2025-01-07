import {UserService} from "../../services/user.service";

export class GetAllGroupsUserIsInUseCase {
    constructor(private readonly userService: UserService) {
    }

    async execute(userId: string) {
        return this.userService.getAllGroupsUserIsIn(userId)
    }
}
