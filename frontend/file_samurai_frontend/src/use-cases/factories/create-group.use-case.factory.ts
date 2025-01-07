import {CreateGroupUseCase} from "../group/create-group-use.case";
import {GroupService} from "../../services/groupService";

export class CreateGroupUseCaseFactory {
    static create() {
        return new CreateGroupUseCase(
            new GroupService()
        )
    }
}