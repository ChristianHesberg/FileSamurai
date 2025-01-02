import {GroupService} from "../services/groupService";

export class GetGroupsForEmailUseCase {


    constructor(private readonly groupService: GroupService) {
    }

    async execute(){
        return await this.groupService.getGroups()
    }
}