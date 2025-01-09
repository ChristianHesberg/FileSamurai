import TableOptionsBtn from "./TableOptionsBtn";
import React, {useState} from "react";
import {faCloudArrowDown} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {FileOption} from "../models/FileOption";
import Modal from "./Modal";
import {EditFileModal} from "./EditFileModal";


interface FileTableProps {
    files: FileOption[]
    setFiles: React.Dispatch<React.SetStateAction<FileOption[]>>
}

const FileTable: React.FC<FileTableProps> = ({files, setFiles}) => {
    const [selectedFile, setSelectedFile] = useState<FileOption | null>(null)


    const addMemberBtn = (fileOption: FileOption) => {
        return (
            <button
                key={"memberBtn" + fileOption.id}
                className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                role="menuitem"
                onClick={() => {
                    setSelectedFile(fileOption)
                }}
            >
                Edit
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
                }}
            >
                Delete File
            </button>
        )
    }
    const isFileSelected = () => {
        return !!selectedFile;
    }
    const clearSelectedFile = () => {
        setSelectedFile(null)
    }
    const buttons = (selectedFile: FileOption) => {
        return <div>
            {downloadBtn(selectedFile)}
            {selectedFile.role == "Editor" ? (
                <>
                    {addMemberBtn(selectedFile)}
                    {deleteBtn(selectedFile)}
                </>
            ) : <></>
            }

        </div>
    }
    return (
        <div>
            {selectedFile ? <Modal isOpen={isFileSelected()} onClose={clearSelectedFile}
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
                            <TableOptionsBtn children={[buttons(f)]}/>
                        </td>
                    </tr>
                )}
                </tbody>
            </table>
        </div>
    )
}

export default FileTable