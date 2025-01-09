import React, {useEffect} from "react";
import {useUseCases} from "../providers/UseCaseProvider";
import {useKey} from "../providers/KeyProvider";
import {useAuth} from "../providers/AuthProvider";
import {FileOption} from "../models/FileOption";

export interface EditFileModal {
    selectedFile: FileOption
}

export const EditFileModal: React.FC<EditFileModal> = ({selectedFile}) => {
    const {decryptFileUseCase} = useUseCases()
    const {retrieveKey} = useKey()
    const {user} = useAuth()
    const x =async () => {
        //new File([a], 'example.txt', { type: 'text/plain' });
    }

    useEffect(() => {
        const key = retrieveKey()
        console.log(key)

     //   a.type
       // decryptFileUseCase.execute(user?.userId!, selectedFile.id, key!).then(buff => console.log(buff))
    }, []);

    return <div>
        <p>File Name: {selectedFile.name}</p>


    </div>
}