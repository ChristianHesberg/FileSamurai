import {GetUserByEmailOrRegisterUseCase} from "../user/get-user-by-email-or-register.use-case";
import {UserService} from "../../services/user.service";

export class GetUserByEmailOrRegisterUseCaseFactory {
    static create(){
        return new GetUserByEmailOrRegisterUseCase(
            new UserService()
        );
    }
}