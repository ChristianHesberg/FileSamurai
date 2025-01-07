import {ValidatePasswordUseCase} from "../user/validate-password.use-case";
import {UserService} from "../../services/user.service";

export class ValidatePasswordUseCaseFactory {
    static create() {
        return new ValidatePasswordUseCase(
            new UserService()
        )
    }
}