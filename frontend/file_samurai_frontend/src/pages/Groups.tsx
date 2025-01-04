import React, {useEffect, useState} from "react";
import {GroupsTable} from "../components/GroupsTable";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPeopleGroup} from "@fortawesome/free-solid-svg-icons";
import {Group} from "../models/Group";
import {useUseCases} from "../providers/UseCaseProvider";

export function Groups() {
    const [newGroup, setNewGroup] = useState<string>("")
    const [groups, setGroups] = useState<Group[]>([])

    const {createGroupUseCase, getGroupsFromEmailUseCase} = useUseCases()
    useEffect(() => {
        getGroupsFromEmailUseCase.execute().then(g => setGroups(g)).catch(e => console.log(e))

    }, [])

    function handleGroupChange(event: React.ChangeEvent<HTMLInputElement>) {
        setNewGroup(event.target.value)
    }

    function onNewGroupClick() {
        if (newGroup.length === 0) return
        createGroupUseCase.execute(newGroup)
            .then((g) => setGroups((prevState) => [...prevState, g]))
            .catch(() => console.log("error lmao"))
        setNewGroup("")
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
                <GroupsTable groups={groups}/>
            </div>

        </div>
    )

}

