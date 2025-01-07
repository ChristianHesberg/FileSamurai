import {GroupService} from "../../services/groupService";

export class GetUsersInGroupUseCase {
    constructor(private readonly groupService: GroupService) {
    }

    async execute(groupId:string){
        return await this.groupService.getUsersInGroup(groupId)
    }
}