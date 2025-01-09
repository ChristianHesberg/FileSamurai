import React, {useEffect, useState} from "react";
import {useUseCases} from "../../providers/UseCaseProvider";
import {useKey} from "../../providers/KeyProvider";
import {useAuth} from "../../providers/AuthProvider";
import {FileOption} from "../../models/FileOption";
import UploadFileBtn from "../UploadFileBtn";
import {Selector} from "../Selector";
import {User} from "../../models/user.model";
import {SelectionOption} from "../../models/selectionOption";
import {Group} from "../../models/Group";

export interface EditFileModal {
    selectedFile: FileOption
}

export const EditFileModal: React.FC<EditFileModal> = ({selectedFile}) => {
    const {decryptFileUseCase} = useUseCases()
    const {retrieveKey} = useKey()
    const {user} = useAuth()
    const [newFile, setNewFile] = useState<File | null>(null)


    const x = async () => {
        //new File([a], 'example.txt', { type: 'text/plain' });
    }





        return <div className={"flex flex-col gap-y-2"}>
            <h1 className={"text-2xl text-center"}>Current File: {selectedFile.name}</h1>
            <UploadFileBtn currentFile={newFile} setFile={setNewFile}/>



        </div>

}