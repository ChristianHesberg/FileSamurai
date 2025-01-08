import {User} from "../models/user.model";

export interface IUserService {
    getUserByToken(): Promise<User>;

    getUserIfNullRegister(email: string): Promise<User>;

    registerUser(email: string, hashedPassword: string, salt: string): Promise<User>;

    validatePassword(password: string): Promise<boolean>;
}