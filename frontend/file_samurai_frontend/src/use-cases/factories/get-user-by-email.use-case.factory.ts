import {GetUserByEmailUseCase} from "../get-user-by-email.use-case";
import {UserService} from "../../services/user.service";

export class GetUserByEmailUseCaseFactory {

    static create() {
        return new GetUserByEmailUseCase(
            new UserService()
        )
    }
}