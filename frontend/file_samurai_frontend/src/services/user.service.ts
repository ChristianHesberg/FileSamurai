import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";
import {UserServiceInterface} from "./user.service.interface";

export class UserService implements UserServiceInterface{
    async getUserByEmail(email: string): Promise<User> {
        const response = await axiosInstance.get<User>(`user/email/${email}`);
        return response.data;
    }

    async getUserIfNullRegister(email: string): Promise<User> {
        const response = await axiosInstance.get<User>(`user/getUserIfNullRegister/${email}`);
        return response.data;
    }

    async registerUser(email: string, password: string): Promise<User> {
        const body = {email: email, Password: password}
        const response = await axiosInstance.post<User>(`user/createUser`, body)
        return response.data
    }

    async validatePassword(password: string): Promise<boolean> {
        const response = await axiosInstance.get<boolean>(`user/validatePassword?password=${password}`)
        return response.data
    }
}

