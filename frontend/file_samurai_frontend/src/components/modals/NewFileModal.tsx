import React, {useEffect, useState} from "react";
import UploadFileBtn from "../UploadFileBtn";
import {Group} from "../../models/Group";
import {useUseCases} from "../../providers/UseCaseProvider";
import {useAuth} from "../../providers/AuthProvider";
import {Selector} from "../Selector";
import {SelectionOption} from "../../models/selectionOption";
import {User} from "../../models/user.model";
import {Buffer} from "buffer";

export const NewFileModal = () => {
    const [groupOptions, setGroupOptions] = useState<SelectionOption[]>([])
    const [selectedGroup, setSelectedGroup] = useState<SelectionOption | null>(null)
    const [groupSearchValue, setGroupSearchValue] = useState<string>("")

    const [userOptions, setUserOptions] = useState<SelectionOption[]>([])
    const [selectedUser, setSelectedUser] = useState<SelectionOption | null>(null)
    const [userSearchValue, setUserSearchValue] = useState<string>("")

    const [file, setFile] = useState<File | null>(null)

    const {getAllGroupsUserIsInUseCase, getUsersInGroupUseCase, createFileUseCase} = useUseCases()
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
        if (selected == null) return
        getUsersInGroupUseCase.execute(selected.value)
            .then(r => {
                const filtered = r.filter(u => u.email != user!.email)
                const formattedOptions = filtered.map((user: User) => ({
                    value: user.id,
                    label: user.email
                }))
                setUserOptions(formattedOptions)
            })
            .catch(e => console.error(e))
    }

    const handleUserChange = (selected: any) => {
        setSelectedUser(selected)
    }

    const isUploadDisabled = !file || !selectedGroup

    function fileToBuffer(file: File): Promise<ArrayBuffer> {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();

            reader.onload = () => {
                if (reader.result instanceof ArrayBuffer) {
                    resolve(reader.result);
                } else {
                    reject(new Error('Failed to convert file to ArrayBuffer'));
                }
            };

            reader.onerror = () => {
                reject(reader.error);
            };

            reader.readAsArrayBuffer(file);
        });
    }

    const handleUploadClick = () => {
        if (!file || !selectedGroup) return
        //get file -> encrypt -> send

        fileToBuffer(file).then(buff => {
            createFileUseCase.execute(user!.userId!, selectedGroup.value, Buffer.from(buff), file.name)
                .then(()=> console.log("yay"))
                .catch(e=> console.log(e))
        })


    };
    return (
        <div className={"flex flex-col space-y-2.5"}>

            <UploadFileBtn setFile={setFile} currentFile={file}/>


            <Selector searchValue={groupSearchValue} options={groupOptions} setSearchValue={setGroupSearchValue}
                      onChange={handleGroupChange} selectedValue={selectedGroup} placeholder={"Select group"}/>

            {selectedGroup ?
                <Selector searchValue={userSearchValue} options={userOptions} setSearchValue={setUserSearchValue}
                          onChange={handleUserChange} selectedValue={selectedUser} isMulti={true}/>
                : <></>}

            <button
                className={"bg-neutral-900 border border-neutral-700 p-2 hover:bg-neutral-700 rounded disabled:bg-red-950"}
                disabled={isUploadDisabled}
                onClick={handleUploadClick}
            >
                Upload file
            </button>

        </div>
    )

}