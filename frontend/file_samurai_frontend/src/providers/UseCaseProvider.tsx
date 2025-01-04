import React, {createContext, ReactNode, useContext} from "react";
import {CreateFileUseCase} from "../use-cases/create-file.use-case";
import {DecryptFileUseCase} from "../use-cases/decrypt-file.use-case";
import {ShareFileUseCase} from "../use-cases/share-file.use-case";
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";
import {CryptographyService} from "../services/cryptography.service";
import {CreateUserKeyPairUseCase} from "../use-cases/create-user-key-pair.use-case";


interface UseCaseProviderProps {
    children: ReactNode
}

interface UseCaseContextType {
    //cryto
    createFileUseCase: CreateFileUseCase,
    decryptFileUseCase: DecryptFileUseCase,
    shareFileUseCase: ShareFileUseCase,
    createUserKeyPairUseCase: CreateUserKeyPairUseCase

    //user stuff

    //group stuff
}

const UseCaseContext = createContext<UseCaseContextType | undefined>(undefined)
export const UseCaseProvider: React.FC<UseCaseProviderProps> = ({children}) => {
    const fileService = new FileService()
    const keyService = new KeyService()
    const cryptoService = new CryptographyService()

    const createFileUseCase = new CreateFileUseCase(fileService, keyService, cryptoService)
    const decryptFileUseCase = new DecryptFileUseCase(fileService, keyService, cryptoService)
    const shareFileUseCase = new ShareFileUseCase(fileService, keyService, cryptoService)
    const createUserKeyPairUseCase = new CreateUserKeyPairUseCase(keyService, cryptoService)

    return (
        <UseCaseContext.Provider value={{createFileUseCase, decryptFileUseCase, shareFileUseCase, createUserKeyPairUseCase}}>
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
