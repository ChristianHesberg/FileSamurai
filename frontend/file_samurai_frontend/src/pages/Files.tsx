import React from "react";
import FileTable from "../components/FileTable";
import Header from "../components/Header";

export function Files() {
    return (

                <div className={"flex-col"}>
                    <h1 className={"text-lg"}>All files</h1>
                    <FileTable/>
                </div>


    )

}