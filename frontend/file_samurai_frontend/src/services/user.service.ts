import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";

export class UserService {
    async getUserByEmail(email: string): Promise<User> {
        const response = await axiosInstance.get<User>(`user/email/${email}`);
        return response.data;
    }

    async getUserIfNullRegister(email: string) {
        const response = await axiosInstance.get<User>(`user/getUserIfNullRegister/${email}`);
        return response.data;
    }

    async registerUser(email: string) {
        const response = await axiosInstance.post<User>(`user/createUser`, {email: email})
    }
}

