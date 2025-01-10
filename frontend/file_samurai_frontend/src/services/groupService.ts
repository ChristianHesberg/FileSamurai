import axiosInstance from "../api/axios-instance";
import {User} from "../models/user.model";

export class GroupService {
    async createGroup(name: string) {
        const body = {name: name}
        const response = await axiosInstance.post("/group", body)
        return response.data
    }

    async getGroups() {
        const response = await axiosInstance.get("/group/groupsForEmail")
        return response.data
    }

    async getUsersInGroup(groupId: string): Promise<User[]> {
        const response = await axiosInstance.get(`group/users/${groupId}`)
        return response.data
    }

    async addUserToGroup(email: string, groupId: string) {
        const body = {
            userEmail: email,
            groupId: groupId
        }
        const response = await axiosInstance.post('group/users', body)
        return response.data
    }

    async removeUserFromGroup(userId: string, groupId: string) {
        const response = await axiosInstance.delete(`group/removeUserFromGroup?groupId=${groupId}&userId=${userId}`)
        return response.data
    }

    async deleteGroup(groupId: string) {
        const response = await axiosInstance.delete(`group/${groupId}`)
        return response.data
    }
}