import React, {useEffect, useState} from "react";
import FileTable from "../components/FileTable";
import UploadFileBtn from "../components/UploadFileBtn";
import {FileDto} from "../models/FileDto";
import Modal from "../components/Modal";
import {NewFileModal} from "../components/NewFileModal";
import {useUseCases} from "../providers/UseCaseProvider";
import {FileOption} from "../models/FileOption";
import {useAuth} from "../providers/AuthProvider";

export function Files() {
    const [files, setFiles] = useState<FileOption[]>([]) //todo get all files on load
    const [modalOpen, setModalOpen] = useState<boolean>(false)
    const {getFileOptionsUseCase} = useUseCases()
    const {user} = useAuth()
    useEffect(() => {
        getFileOptionsUseCase.execute(user?.userId!).then(r => {
            setFiles(r)
        }).catch(e => console.error(e))
    }, []);


    const btn = () => {
        return (
            <button
                onClick={() => setModalOpen(true)}
                className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                New File
            </button>
        )

    }

    //Get all files -> into table

    return (
        <div className={"flex-col"}>
            <div className={"flex justify-between"}>
                <h1 className={"text-lg"}>All files</h1>
                {btn()}
            </div>
            <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} child={<NewFileModal/>}/>
            <FileTable files={files} setFiles={setFiles}/>
        </div>
    )

}