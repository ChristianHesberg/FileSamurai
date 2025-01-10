import React, {useEffect, useState} from "react";
import FileTable from "../components/FileTable";
import UploadFileBtn from "../components/UploadFileBtn";
import {FileDto} from "../models/FileDto";
import Modal from "../components/Modal";
import {NewFileModal} from "../components/modals/NewFileModal";
import {useUseCases} from "../providers/UseCaseProvider";
import {FileOption} from "../models/FileOption";
import {useAuth} from "../providers/AuthProvider";

export function Files() {
    const [files, setFiles] = useState<FileOption[]>([])
    const [modalOpen, setModalOpen] = useState<boolean>(false)
    const {getFileOptionsUseCase} = useUseCases()
    const {user} = useAuth()

    useEffect(() => {
        createFileOptions()
    }, []);

    const createFileOptions = ()=> {
        getFileOptionsUseCase.execute(user?.userId!).then(r => {
            setFiles(r)
        }).catch(e => console.error(e))
    }

    const btn = () => {
        return (
            <button
                onClick={() => setModalOpen(true)}
                className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                New File
            </button>
        )

    }
    const onClose = () => {
        createFileOptions()
        setModalOpen(false)
    }

    return (
        <div className={"flex-col"}>
            <div className={"flex justify-between"}>
                <h1 className={"text-lg"}>All files</h1>
                {btn()}
            </div>
            <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)}
                   child={<NewFileModal onClose={onClose} />}/>
            <FileTable files={files} setFiles={setFiles}/>
        </div>
    )

}