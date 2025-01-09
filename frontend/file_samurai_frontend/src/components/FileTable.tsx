import TableOptionsBtn from "./TableOptionsBtn";
import React, {useState} from "react";
import {faCloudArrowDown} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {FileOption} from "../models/FileOption";
import Modal from "./Modal";
import {EditFileModal} from "./modals/EditFileModal";
import {useUseCases} from "../providers/UseCaseProvider";
import {useKey} from "../providers/KeyProvider";
import {useAuth} from "../providers/AuthProvider";
import {ShareFileModal} from "./modals/ShareFileModal";


interface FileTableProps {
    files: FileOption[]
    setFiles: React.Dispatch<React.SetStateAction<FileOption[]>>
}

const FileTable: React.FC<FileTableProps> = ({files, setFiles}) => {
    const [selectedFile, setSelectedFile] = useState<FileOption | null>(null)
    const [openShareModal, setOpenShareModal] = useState<boolean>(false)
    const [openEditModal, setOpenEditModal] = useState<boolean>(false)
    const {decryptFileUseCase, downloadFileUseCase, deleteFileUseCase} = useUseCases();
    const {retrieveKey} = useKey();
    const {user} = useAuth()


    const editBtn = (fileOption: FileOption) => {
        return (
            <button
                key={"memberBtn" + fileOption.id}
                className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                role="menuitem"
                onClick={() => {
                    setSelectedFile(fileOption)
                    setOpenEditModal(true)
                }}
            >
                Edit
            </button>
        )

    }

    const shareBtn = (fileOption: FileOption) => {
        return (
            <button
                key={"shareBtn" + fileOption.id}
                className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                role="menuitem"
                onClick={() => {
                    setSelectedFile(fileOption)
                    setOpenShareModal(true)
                }}
            >
                Share
            </button>
        )
    }
    const downloadBtn = (fileOption: FileOption) => {
        return (
            <button
                key={"downloadBtn" + fileOption.id}
                className="block px-4 py-2 text-sm bg-sky-600 hover:bg-sky-400 w-full rounded"
                role="menuitem"
                onClick={() => {
                    const key = retrieveKey();
                    if (key == null) {
                        console.log('aint got no key');
                        return;
                    }
                    decryptFileUseCase.execute(user?.userId!, fileOption.id, key)
                        .then((file) => downloadFileUseCase.execute(file, fileOption.name));
                }}
            >
                Download
                <FontAwesomeIcon icon={faCloudArrowDown} size={"xl"}/>
            </button>
        )

    }
    const deleteBtn = (fileOption: FileOption) => {
        return (
            <button
                key={"deleteBtn" + fileOption.id}
                className="block px-4 py-2 text-sm bg-red-900 hover:bg-red-800 w-full rounded"
                role="menuitem"
                onClick={() => {
                    deleteFileUseCase.execute(fileOption.id)
                        .then(() => setFiles(prevState => prevState.filter(x => x.id !== fileOption.id)))
                        .catch(e => console.log(e))
                }}
            >
                Delete File
            </button>
        )
    }
    const clearSelectedFile = (setClose: any) => {
        setSelectedFile(null)
        setClose(false)
    }
    const buttons = (selectedFile: FileOption) => {
        return <div>
            {downloadBtn(selectedFile)}
            {selectedFile.role == "Editor" ? (
                <>
                    {editBtn(selectedFile)}
                    {shareBtn(selectedFile)}
                    {deleteBtn(selectedFile)}
                </>
            ) : <></>
            }

        </div>
    }
    return (
        <div>
            {selectedFile ? <Modal isOpen={openShareModal} onClose={() => clearSelectedFile(setOpenShareModal)}
                                   child={<ShareFileModal selectedFile={selectedFile}
                                                          onClose={() => clearSelectedFile(setOpenShareModal)}/>}/> : <></>}
            {selectedFile ? <Modal isOpen={openEditModal} onClose={() => clearSelectedFile(setOpenEditModal)}
                                   child={<EditFileModal selectedFile={selectedFile}/>}/> : <></>}

            <table className={"min-w-full text-left text-sm font-light text-surface dark:text-white"}>
                <thead className={"border-b border-neutral-200 font-medium dark:border-white/10"}>
                <tr>
                    <th scope={"col"} className={"px-6 py-4"}>
                        Name
                    </th>
                    <th scope={"col"} className={"px-6 py-4"}>
                    </th>
                </tr>
                </thead>
                <tbody>
                {files.map((f, index) =>
                    <tr key={index}
                        className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600 group"}>
                        <td>
                            {f.name}
                        </td>
                        <td className={"px-6 py-4"}>
                            <TableOptionsBtn key={index} children={[buttons(f)]}/>
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
        </div>
    )
}

export default FileTable