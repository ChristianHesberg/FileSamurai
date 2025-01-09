import React, {useRef, useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUpload} from "@fortawesome/free-solid-svg-icons";
import {AddFileDto} from "../models/addFileDto";
import {log} from "node:util";

interface NewFileBtnProps {
    currentFile: File | null
    setFile: React.Dispatch<React.SetStateAction<File | null>>
}

const UploadFileBtn: React.FC<NewFileBtnProps> = ({setFile, currentFile}) => {
    const fileInputRef = useRef<HTMLInputElement | null>(null);
    const [fileName, setFileName] = useState<string>("")

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const files = event.target.files;
        if (files && files.length > 0) {
            const pickedFile = files[0]
            console.log(pickedFile)
            setFileName(pickedFile.name)
            setFile(pickedFile)
        }
    };

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!currentFile) return
        const value = e.target.value
        setFileName(value)
        setFile((prevState) => prevState ? {...prevState, name: value} : null);

    }

    const openFilePicker = () => {
        fileInputRef.current?.click();
    };

    return (
        <div className={"flex flex-row justify-center items-center space-x-2"}>
            <button onClick={openFilePicker}
                    className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                File
                <FontAwesomeIcon icon={faUpload} size={"xl"}></FontAwesomeIcon>
                <input
                    className={"hidden"}
                    type="file"
                    ref={fileInputRef}
                    accept={".jpg,.png,.txt,.pdf"}
                    onChange={handleFileChange}
                />
            </button>

            <input type="text"
                   value={fileName}
                   onChange={handleNameChange}
                   className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                   placeholder="New File Name"
            />
        </div>
    )
};

export default UploadFileBtn;


