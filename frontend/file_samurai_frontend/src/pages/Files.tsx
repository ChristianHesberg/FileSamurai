import React from "react";
import FileTable from "../components/FileTable";
import UploadFileBtn from "../components/UploadFileBtn";

export function Files() {
    return (
                <div className={"flex-col"}>
                    <div className={"flex justify-between"}>
                        <h1 className={"text-lg"}>All files</h1>
                        <UploadFileBtn/>
                    </div>
                    <FileTable/>
                </div>
    )

}