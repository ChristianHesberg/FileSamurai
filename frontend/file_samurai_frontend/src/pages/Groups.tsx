import React from "react";
import {GroupsTable} from "../components/GroupsTable";
import {useUseCases} from "../providers/UseCaseProvider";

export function Groups() {
    const { createFileUseCase, decryptFileUseCase, shareFileUseCase, createUserKeyPairUseCase } = useUseCases();

    createUserKeyPairUseCase.execute("password", "cchesberg@gmail.com", "ff4fe9ac-daca-4c55-9a76-e699fe12b60f").then(
        () => {
            console.log("key pair created");
            createFileUseCase.execute("ff4fe9ac-daca-4c55-9a76-e699fe12b60f", "febb1782-d0ab-472d-867f-cfef488a67b1", Buffer.from("my_cool_file"), "cool title");
        }
    );
    return (
        <div>
            <div className={"flex-col"}>
                <h1 className={"text-lg"}>All your groups</h1>
                <GroupsTable/>
            </div>

        </div>
    )

}

