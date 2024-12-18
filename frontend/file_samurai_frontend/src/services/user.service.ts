import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";

export async function getUserByEmail(email: string): Promise<User> {
    const response = await axiosInstance.get<User>(`user/email/${email}`);
    return response.data;
}

export async function getUserIfNullRegister(email:string) {
    const response = await axiosInstance.get<User>(`user/getUserIfNullRegister/${email}`);
    return response.data;
}