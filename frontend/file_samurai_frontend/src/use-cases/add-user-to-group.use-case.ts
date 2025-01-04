import {GroupService} from "../services/groupService";

export class AddUserToGroupUseCase {
    constructor(private readonly groupService: GroupService) {
    }

    async execute(email: string, groupId: string) {
        return await this.groupService.addUserToGroup(email, groupId)
    }
}