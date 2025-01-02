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

}