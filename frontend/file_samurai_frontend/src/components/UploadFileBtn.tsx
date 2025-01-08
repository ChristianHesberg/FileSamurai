import React, {useRef} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUpload} from "@fortawesome/free-solid-svg-icons";
import {useKey} from "../providers/KeyProvider";


const UploadFileBtn: React.FC = () => {
    const fileInputRef = useRef<HTMLInputElement | null>(null);


    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const files = event.target.files;
        if (files && files.length > 0) {
            // TODO ONLY ALLOW JPG/PNG/PDF OTHER ALLOWED?
            //TODO ENCRYPT SELECTED. MABYE MAKE LOADING BAR
            console.log("Selected file:", files[0].name);
        }
    };

    const openFilePicker = () => {
        fileInputRef.current?.click();
    };

    return (
            <button onClick={openFilePicker}
                    className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                Upload file
                <FontAwesomeIcon icon={faUpload} size={"xl"}></FontAwesomeIcon>
                <input
                    className={"hidden"}
                    type="file"
                    ref={fileInputRef}
                    onChange={handleFileChange}
                />
            </button>
    );
};

export default UploadFileBtn;


