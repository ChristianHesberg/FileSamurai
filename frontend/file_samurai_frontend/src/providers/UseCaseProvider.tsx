import React, {createContext, ReactNode, useContext} from "react";
import {CreateFileUseCase} from "../use-cases/create-file.use-case";
import {DecryptFileUseCase} from "../use-cases/decrypt-file.use-case";
import {ShareFileUseCase} from "../use-cases/share-file.use-case";
import {FileService} from "../services/file.service";
import {KeyService} from "../services/key.service";


interface UseCaseProviderProps {
    children: ReactNode
}

interface UseCaseContextType {
    //cryto
    //createFileUseCase: CreateFileUseCase,

    //user stuff

    //group stuff
}

const UseCaseContext = createContext<UseCaseContextType | undefined>(undefined)
export const UseCaseProvider: React.FC<UseCaseProviderProps> = ({children}) => {
    const fileService = new FileService()
    const keyService = new KeyService()
    //const cryptoService = new ClientSideCryptographyService()

    //const createFileUseCase = new CreateFileUseCase(fileService, keyService, cryptoService)

    return (
        <UseCaseContext.Provider value={{}}>
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
