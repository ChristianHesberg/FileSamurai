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

import {GetAllGroupsUserIsInUseCase} from "../use-cases/user/get-all-groups-user-is-in.use-case";
import {RegisterUserUseCase} from "../use-cases/user/register-user.use-case";
import {GetUserByTokenUseCase} from "../use-cases/user/get-user-by-token-use.case";

import {DeriveEncryptionKeyUseCase} from "../use-cases/keys/derive-encryption-key.use-case";
import {ValidatePasswordHashUseCase} from "../use-cases/keys/validate-password-hash.use-case";
import {GeneratePasswordHashUseCase} from "../use-cases/keys/generate-password-hash.use-case";
import {GetFileOptionsUseCase} from "../use-cases/file/get-file-options.use-case";
import {DownloadFileUseCase} from "../use-cases/file/download-file-use.case";
import {GetAllFileAccessUseCase} from "../use-cases/file/get-all-file-access.use-case";
import {ShareFileWithMultipleUsersUseCase} from "../use-cases/file/share-file-with-multiple-users.use-case";
import {DeleteUserFileAccessUseCase} from "../use-cases/file/delete-user-file-access.use-case";
import {DeleteFileUseCase} from "../use-cases/file/delete-file.use-case";


interface UseCaseProviderProps {
    children: ReactNode
}

interface UseCaseContextType {
    //keys
    generatePasswordHashUseCase: GeneratePasswordHashUseCase
    validatePasswordHashUseCase: ValidatePasswordHashUseCase
    deriveEncryptionKeyUseCase: DeriveEncryptionKeyUseCase
    // createFileUseCase: CreateFileUseCase,

    createFileUseCase: CreateFileUseCase,
    decryptFileUseCase: DecryptFileUseCase,
    shareFileUseCase: ShareFileUseCase,
    shareFileWithMultipleUsersUseCase: ShareFileWithMultipleUsersUseCase
    createUserKeyPairUseCase: CreateUserKeyPairUseCase,

    //user stuff
    registerUserUseCase: RegisterUserUseCase,
    getUserByTokenUseCase: GetUserByTokenUseCase
    getAllGroupsUserIsInUseCase: GetAllGroupsUserIsInUseCase,

    //group stuff
    createGroupUseCase: CreateGroupUseCase,
    getGroupsFromEmailUseCase: GetGroupsForEmailUseCase,
    getUsersInGroupUseCase: GetUsersInGroupUseCase,
    addUserToGroupUseCase: AddUserToGroupUseCase,
    removeUserFromGroup: RemoveUserFromGroupUseCase,
    deleteGroupUseCase: DeleteGroupUseCase

    //file
    getFileOptionsUseCase: GetFileOptionsUseCase
    downloadFileUseCase: DownloadFileUseCase,
    getAllFileAccessUseCase: GetAllFileAccessUseCase
    deleteUserFileAccessUseCase: DeleteUserFileAccessUseCase,
    deleteFileUseCase:DeleteFileUseCase
}

const UseCaseContext = createContext<UseCaseContextType | undefined>(undefined)


export const UseCaseProvider: React.FC<UseCaseProviderProps> = ({children}) => {
    const fileService = new FileService()
    const keyService = new KeyService()
    const cryptoService = new CryptographyService()
    const groupService = new GroupService()
    const userService = new UserService()

    //Key
    const deriveEncryptionKeyUseCase = new DeriveEncryptionKeyUseCase(cryptoService, keyService);
    const generatePasswordHashUseCase = new GeneratePasswordHashUseCase(cryptoService)
    const validatePasswordHashUseCase = new ValidatePasswordHashUseCase(cryptoService, userService)

    //Group
    const createGroupUseCase = new CreateGroupUseCase(groupService);
    const getGroupsFromEmailUseCase = new GetGroupsForEmailUseCase(groupService);
    const getUsersInGroupUseCase = new GetUsersInGroupUseCase(groupService);
    const addUserToGroupUseCase = new AddUserToGroupUseCase(groupService);
    const removeUserFromGroup = new RemoveUserFromGroupUseCase(groupService);
    const deleteGroupUseCase = new DeleteGroupUseCase(groupService)

    //crypto
    const createFileUseCase = new CreateFileUseCase(fileService, keyService, cryptoService)
    const decryptFileUseCase = new DecryptFileUseCase(fileService, keyService, cryptoService)
    const shareFileUseCase = new ShareFileUseCase(fileService, keyService, cryptoService)
    const shareFileWithMultipleUsersUseCase = new ShareFileWithMultipleUsersUseCase(fileService, keyService, cryptoService)
    const createUserKeyPairUseCase = new CreateUserKeyPairUseCase(keyService, cryptoService)

    //user
    const getAllGroupsUserIsInUseCase = new GetAllGroupsUserIsInUseCase(userService)
    const getUserByTokenUseCase = new GetUserByTokenUseCase(userService)
    const registerUserUseCase = new RegisterUserUseCase(userService)

    //File
    const getFileOptionsUseCase = new GetFileOptionsUseCase(fileService);
    const downloadFileUseCase = new DownloadFileUseCase();
    const getAllFileAccessUseCase = new GetAllFileAccessUseCase(fileService)
    const deleteUserFileAccessUseCase = new DeleteUserFileAccessUseCase(fileService)
    const deleteFileUseCase = new DeleteFileUseCase(fileService)
    return (
        <UseCaseContext.Provider value={{
            createFileUseCase,
            decryptFileUseCase,
            shareFileUseCase,
            createUserKeyPairUseCase,
            getGroupsFromEmailUseCase,
            createGroupUseCase,
            getUsersInGroupUseCase,
            addUserToGroupUseCase,
            removeUserFromGroup,
            deleteGroupUseCase,
            getAllGroupsUserIsInUseCase,
            getUserByTokenUseCase,
            registerUserUseCase,
            deriveEncryptionKeyUseCase,
            validatePasswordHashUseCase,
            generatePasswordHashUseCase,
            getFileOptionsUseCase,
            downloadFileUseCase,
            getAllFileAccessUseCase,
            shareFileWithMultipleUsersUseCase,
            deleteUserFileAccessUseCase,
            deleteFileUseCase
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
