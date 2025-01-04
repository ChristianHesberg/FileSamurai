import axiosInstance from "../api/axios-instance";

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

    async getUsersInGroup(groupId: string) {
        const response = await axiosInstance.get(`group/usersInGroup?groupid=${groupId}`)
        return response.data
    }

    async addUserToGroup(email: string, groupId: string) {
        const body = {
            userEmail: email,
            groupId: groupId
        }
        const response = await axiosInstance.post('group/addUser',body)
        return response.data
    }

}