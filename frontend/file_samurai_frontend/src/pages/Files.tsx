import React, {useState} from "react";
import FileTable from "../components/FileTable";
import UploadFileBtn from "../components/UploadFileBtn";
import {FileDto} from "../models/FileDto";
import Modal from "../components/Modal";
import {NewFileModal} from "../components/NewFileModal";

export function Files() {
    const [files, setFiles] = useState<FileDto[]>([]) //todo get all files on load
    const [modalOpen, setModalOpen] = useState<boolean>(false)
    const btn = () => {
        return (
            <button
                onClick={() => setModalOpen(true)}
                className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                New File
            </button>
        )

    }

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