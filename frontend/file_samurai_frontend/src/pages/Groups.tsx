import React, {useEffect, useState} from "react";
import {GroupsTable} from "../components/GroupsTable";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPeopleGroup} from "@fortawesome/free-solid-svg-icons";
import {Group} from "../models/Group";
import {useUseCases} from "../providers/UseCaseProvider";
import {Buffer} from 'buffer';
import {DeriveEncryptionKeyUseCase} from "../use-cases/file/derive-encryption-key.use-case";
import {useKey} from "../providers/KeyProvider";

export function Groups() {
    const [newGroup, setNewGroup] = useState<string>("")
    const [groups, setGroups] = useState<Group[]>([])

    const {createGroupUseCase, getGroupsFromEmailUseCase, createFileUseCase, decryptFileUseCase, deriveEncryptionKeyUseCase} = useUseCases()
    const { storeKey } = useKey();
    useEffect(() => {
        getGroupsFromEmailUseCase.execute().then(g => setGroups(g)).catch(e => console.log(e))

    }, [])

    function handleGroupChange(event: React.ChangeEvent<HTMLInputElement>) {
        setNewGroup(event.target.value)
    }

    function onNewGroupClick() {
        let id ='';
        createFileUseCase.execute('2f486cba-c664-4689-acf3-6da89df99355', "a17c7a4c-b31f-4c46-adf5-6bdfd7b1969c", Buffer.from('my_text'), 'title')
            .then((v) => {
                console.log(v)
                id = v.id;
                deriveEncryptionKeyUseCase.execute('cchesberg@gmail.com-password', '2f486cba-c664-4689-acf3-6da89df99355').then((v) => {
                    storeKey(v);
                    }
                )
            });
        /*if (newGroup.length === 0) return
        createGroupUseCase.execute(newGroup)
            .then((g) => setGroups((prevState) => [...prevState, g]))
            .catch((e) => console.log(e))
        setNewGroup("")*/
    }

    return (
        <div>
            <div className={"flex-col"}>
                <h1 className={"text-lg"}>All your groups</h1>
                <div className={"flex flex-row justify-center items-center"}>
                    <input id={"groupInput"}
                           type="text"
                           className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
                             focus-visible:ring-ring flex h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
                             focus-visible:ring-2 focus-visible:ring-offset-2"
                           placeholder="New Group Name"
                           value={newGroup}
                           onChange={handleGroupChange}

                    />
                    <button onClick={onNewGroupClick}
                            className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded"}>
                        New Group
                        <FontAwesomeIcon icon={faPeopleGroup} size={"xl"}/>
                    </button>
                </div>
                <GroupsTable groups={groups} setGroups={setGroups}/>
            </div>

        </div>
    )

}

