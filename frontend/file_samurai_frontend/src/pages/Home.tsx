import React from "react";
import {useAuth} from "../providers/AuthProvider";
import {Navigate} from "react-router-dom";
import SettingsDropdown from "../components/SettingsDropdown";
import FileTable from "../components/FileTable";

export function Home() {
    const {user, logout} = useAuth();


    return (
        <div>
            {user ? (

                <div className={"flex-col "}>
                    <div className={"flex justify-end w-full"}>
                        <SettingsDropdown/>
                    </div>
                    <h1 className={"text-lg"}>All files</h1>
                    <FileTable/>

                </div>
            ) : (<Navigate to={"/"}/>)
            }
        </div>
    )

}