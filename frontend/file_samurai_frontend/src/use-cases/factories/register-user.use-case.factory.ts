import {RegisterUserUseCase} from "../register-user.use-case";
import {UserService} from "../../services/user.service";

export class RegisterUserUseCaseFactory {
    static create() {
        return new RegisterUserUseCase(
            new UserService()
        )
    }
}