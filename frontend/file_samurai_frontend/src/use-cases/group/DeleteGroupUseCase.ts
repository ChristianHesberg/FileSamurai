import {GroupService} from "../../services/groupService";

export class DeleteGroupUseCase {
    constructor(private readonly groupService: GroupService) {
    }

    async execute(groupId: string) {
        return await this.groupService.deleteGroup(groupId)
    }
}