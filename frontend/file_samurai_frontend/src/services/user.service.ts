import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";
import {IUserService} from "./IUserService";
import {Group} from "../models/Group";

export class UserService implements IUserService {
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

    async getAllGroupsUserIsIn(userId: string): Promise<Group[]> {
        const response = await axiosInstance.get(`user/groups/${userId}`)
        return response.data
    }

    async getAllUsersInGroup(groupId: string) {
        const response = await axiosInstance.get(`users/inGroup/${groupId}`)
        return response.data
    }
}

