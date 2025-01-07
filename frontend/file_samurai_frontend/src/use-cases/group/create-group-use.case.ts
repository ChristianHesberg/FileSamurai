import {GroupService} from "../../services/groupService";

export class CreateGroupUseCase {


    constructor(private readonly groupService: GroupService) {
    }

    async execute(name:string){
        return await this.groupService.createGroup(name)
    }
}