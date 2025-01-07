import {User} from "../models/user.model";

export interface IUserService {
    getUserByEmail(email: string): Promise<User>;
    getUserIfNullRegister(email: string): Promise<User>;
    registerUser(email: string, password: string): Promise<User>;
    validatePassword(password: string): Promise<boolean>;
}