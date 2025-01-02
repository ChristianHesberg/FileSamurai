import {GroupService} from "../services/groupService";

export class createGroupUseCase {


    constructor(private readonly groupService: GroupService) {
    }

    async execute(name:string){
        return await this.groupService.createGroup(name)
    }
}