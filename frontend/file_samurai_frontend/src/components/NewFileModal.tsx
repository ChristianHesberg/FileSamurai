import React, {useEffect, useState} from "react";
import UploadFileBtn from "./UploadFileBtn";
import {Group} from "../models/Group";
import {useUseCases} from "../providers/UseCaseProvider";
import {useAuth} from "../providers/AuthProvider";
import {Selector} from "./Selector";
import {SelectionOption} from "../models/selectionOption";
import {User} from "../models/user.model";


export const NewFileModal = () => {
    const [fileName, setFileName] = useState<string>("")
    const [groupOptions, setGroupOptions] = useState<SelectionOption[]>([])
    const [selectedGroup, setSelectedGroup] = useState<SelectionOption | null>(null)
    const [groupSearchValue, setGroupSearchValue] = useState<string>("")

    const [userOptions, setUserOptions] = useState<SelectionOption[]>([])
    const [selectedUser, setSelectedUser] = useState<SelectionOption | null>(null)
    const [userSearchValue, setUserSearchValue] = useState<string>("")

    const {getAllGroupsUserIsInUseCase, getUsersInGroupUseCase} = useUseCases()
    const {user} = useAuth()

    useEffect(() => {
        const userId = user?.userId!
        getAllGroupsUserIsInUseCase.execute(userId)
            .then(r => {
                const formattedOptions = r.map((group: Group) => ({
                    value: group.id,
                    label: group.name,
                }));

                setGroupOptions(formattedOptions);
            })
            .catch(e => console.error(e))
    }, []);

    const handleGroupChange = (selected: any) => {
        setSelectedUser(null)
        setSelectedGroup(selected)

        getUsersInGroupUseCase.execute(selected.value)
            .then(r => {
                const formattedOptions = r.map((user:User) => ({
                    value:user.id,
                    label: user.email
                }))
                setUserOptions(formattedOptions)
            })
            .catch(e => console.error(e))
    }

    const handleUserChange = (selected: any) => {
        setSelectedUser(selected)
    }


    return (
        <div className={"flex flex-col space-y-2.5"}>
            <div className={"flex flex-row justify-center items-center space-x-2"}>
                <UploadFileBtn/>
                <input type="text"
                       value={fileName}
                       onChange={e => setFileName(e.target.value)}
                       className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                       placeholder="File Name"
                />
            </div>

            <Selector searchValue={groupSearchValue} options={groupOptions} setSearchValue={setGroupSearchValue}
                      onChange={handleGroupChange} selectedValue={selectedGroup} placeholder={"Select group"}/>

            {selectedGroup ?
                <Selector searchValue={groupSearchValue} options={userOptions} setSearchValue={setUserSearchValue}
                          onChange={handleUserChange} selectedValue={selectedUser} isMulti={true}/>
                : <></>}

        </div>
    )

}