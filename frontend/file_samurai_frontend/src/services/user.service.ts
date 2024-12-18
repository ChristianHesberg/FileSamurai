import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";

export async function getUserByEmail(email: string): Promise<User> {
    const response = await axiosInstance.get<User>(`user/email/${email}`);
    return response.data;
}

getUserByEmail('cool@email.com').then(res => {
    console.log(res)
});