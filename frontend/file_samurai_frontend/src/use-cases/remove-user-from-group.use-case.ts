import {GroupService} from "../services/groupService";

export class RemoveUserFromGroupUseCase {
    constructor(private readonly groupService: GroupService) {
    }

    async execute(groupId:string,userId:string){
        return await this.groupService.removeUserFromGroup(userId,groupId)
    }
}