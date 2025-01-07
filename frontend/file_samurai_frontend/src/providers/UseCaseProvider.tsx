import React, {createContext, ReactNode, useContext} from "react";

import {CreateGroupUseCase} from "../use-cases/group/create-group-use.case";
import {GetGroupsForEmailUseCase} from "../use-cases/group/get-groups-for-email.use-case";
import {GroupService} from "../services/groupService";
import {UserService} from "../services/user.service";
import {GetUsersInGroupUseCase} from "../use-cases/group/get-users-in-group.use-case";
import {AddUserToGroupUseCase} from "../use-cases/group/add-user-to-group.use-case";
import {RemoveUserFromGroupUseCase} from "../use-cases/group/remove-user-from-group.use-case";

import {CreateFileUseCase} from "../use-cases/file/create-file.use-case";
import {DecryptFileUseCase} from "../use-cases/file/decrypt-file.use-case";
import {ShareFileUseCase} from "../use-cases/file/share-file.use-case";
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {CryptographyService} from "../services/cryptography.service";
import {CreateUserKeyPairUseCase} from "../use-cases/file/create-user-key-pair.use-case";
import {DeleteGroupUseCase} from "../use-cases/group/DeleteGroupUseCase";

interface UseCaseProviderProps {
    children: ReactNode
}

interface UseCaseContextType {
    //cryto

    // createFileUseCase: CreateFileUseCase,

    createFileUseCase: CreateFileUseCase,
    decryptFileUseCase: DecryptFileUseCase,
    shareFileUseCase: ShareFileUseCase,
    createUserKeyPairUseCase: CreateUserKeyPairUseCase

    //user stuff

    //group stuff
    createGroupUseCase: CreateGroupUseCase
    getGroupsFromEmailUseCase: GetGroupsForEmailUseCase,
    getUsersInGroup: GetUsersInGroupUseCase,
    addUserToGroupUseCase: AddUserToGroupUseCase,
    removeUserFromGroup: RemoveUserFromGroupUseCase,
    deleteGroupUseCase: DeleteGroupUseCase
}

const UseCaseContext = createContext<UseCaseContextType | undefined>(undefined)


export const UseCaseProvider: React.FC<UseCaseProviderProps> = ({children}) => {
    const fileService = new FileService()
    const keyService = new KeyService()
    const cryptoService = new CryptographyService()
    const groupService = new GroupService()
    const userService = new UserService()

    //Group
    const createGroupUseCase = new CreateGroupUseCase(groupService);
    const getGroupsFromEmailUseCase = new GetGroupsForEmailUseCase(groupService);
    const getUsersInGroup = new GetUsersInGroupUseCase(groupService);
    const addUserToGroupUseCase = new AddUserToGroupUseCase(groupService);
    const removeUserFromGroup = new RemoveUserFromGroupUseCase(groupService);
    const deleteGroupUseCase = new DeleteGroupUseCase(groupService)

    const createFileUseCase = new CreateFileUseCase(fileService, keyService, cryptoService)
    const decryptFileUseCase = new DecryptFileUseCase(fileService, keyService, cryptoService)
    const shareFileUseCase = new ShareFileUseCase(fileService, keyService, cryptoService)
    const createUserKeyPairUseCase = new CreateUserKeyPairUseCase(keyService, cryptoService)

    return (
        <UseCaseContext.Provider value={{
            createFileUseCase,
            decryptFileUseCase,
            shareFileUseCase,
            createUserKeyPairUseCase,
            getGroupsFromEmailUseCase,
            createGroupUseCase,
            getUsersInGroup,
            addUserToGroupUseCase,
            removeUserFromGroup,
            deleteGroupUseCase
        }}>
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
