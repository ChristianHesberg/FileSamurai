import React from "react";
import {GroupsTable} from "../components/GroupsTable";

export function Groups() {
    return (
        <div>
            <div className={"flex-col"}>
                <h1 className={"text-lg"}>All your groups</h1>
                <GroupsTable/>
            </div>

        </div>
    )

}

