import {createGroupUseCase} from "../create-group.use-case";
import {GroupService} from "../../services/groupService";

export class CreateGroupUseCaseFactory {
    static create() {
        return new createGroupUseCase(
            new GroupService()
        )
    }
}