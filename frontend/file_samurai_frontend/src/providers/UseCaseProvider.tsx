import React, {createContext, ReactNode, useContext} from "react";
import {CreateGroupUseCase} from "../use-cases/create-group-use.case";
import {GetGroupsForEmailUseCase} from "../use-cases/get-groups-for-email.use-case";
import {GroupService} from "../services/groupService";
import {UserService} from "../services/user.service";
import {GetUsersInGroupUseCase} from "../use-cases/get-users-in-group.use-case";
import {AddUserToGroupUseCase} from "../use-cases/add-user-to-group.use-case";

interface UseCaseProviderProps {
    children: ReactNode
}

interface UseCaseContextType {
    //cryto
    // createFileUseCase: CreateFileUseCase,

    //user stuff

    //group stuff
    createGroupUseCase: CreateGroupUseCase
    getGroupsFromEmailUseCase: GetGroupsForEmailUseCase,
    getUsersInGroup: GetUsersInGroupUseCase
    addUserToGroupUseCase: AddUserToGroupUseCase
}

const UseCaseContext = createContext<UseCaseContextType | undefined>(undefined)
export const UseCaseProvider: React.FC<UseCaseProviderProps> = ({children}) => {
    //const fileService = new FileService()
    // const keyService = new KeyService()
    //const cryptoService = new CryptographyService()
    const groupService = new GroupService()
    const userService = new UserService()

    //const createFileUseCase = new CreateFileUseCase(fileService, keyService, cryptoService)
    //Group
    const createGroupUseCase = new CreateGroupUseCase(groupService);
    const getGroupsFromEmailUseCase = new GetGroupsForEmailUseCase(groupService);
    const getUsersInGroup = new GetUsersInGroupUseCase(groupService);
    const addUserToGroupUseCase = new AddUserToGroupUseCase(groupService);

    return (
        <UseCaseContext.Provider
            value={{getGroupsFromEmailUseCase, createGroupUseCase, getUsersInGroup, addUserToGroupUseCase}}>
            {children}
        </UseCaseContext.Provider>

    )
}

export const useUseCases = (): UseCaseContextType => {
    const context = useContext(UseCaseContext)
    if (!context) {
        throw new Error("useUseCases must be used within a useCaseProvider")
    }
    return context
}
