import {CreateGroupUseCase} from "../create-group-use.case";
import {GroupService} from "../../services/groupService";
import {GetGroupsForEmailUseCase} from "../get-groups-for-email.use-case";

export class GetGroupsForEmailUseCaseFactory {
    static create() {
        return new GetGroupsForEmailUseCase(
            new GroupService()
        )
    }
}