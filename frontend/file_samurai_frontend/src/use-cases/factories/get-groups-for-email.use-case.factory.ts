import {CreateGroupUseCase} from "../group/create-group-use.case";
import {GroupService} from "../../services/groupService";
import {GetGroupsForEmailUseCase} from "../group/get-groups-for-email.use-case";

export class GetGroupsForEmailUseCaseFactory {
    static create() {
        return new GetGroupsForEmailUseCase(
            new GroupService()
        )
    }
}