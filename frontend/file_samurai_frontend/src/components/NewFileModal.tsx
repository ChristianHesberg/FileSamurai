import React, {useEffect, useState} from "react";
import UploadFileBtn from "./UploadFileBtn";
import {Group} from "../models/Group";
import {useUseCases} from "../providers/UseCaseProvider";
import {useAuth} from "../providers/AuthProvider";
import {GroupSelector} from "./GroupSelector";
import {SelectionOption} from "../models/selectionOption";


export const NewFileModal = () => {
    const [fileName, setFileName] = useState<string>("")
    const [groupOptions, setGroupOptions] = useState<SelectionOption[]>([])
    const [selectedGroup, setSelectedGroup] = useState<SelectionOption | null>(null)
    const [searchValue, setSearchValue] = useState<string>("")


    const {getAllGroupsUserIsInUseCase} = useUseCases()
    const {user} = useAuth()

    useEffect(() => {
        const a = user?.userId!
        getAllGroupsUserIsInUseCase.execute(a)
            .then(r => {
                const formattedOptions = r.map((group: Group) => ({
                    value: group.id,
                    label: group.name,
                }));

                setGroupOptions(formattedOptions);
            })
            .catch(e => console.error(e))


    }, []);


    /*
    - get all groups i am in
    - create dropdown

    -from selected group, add members. ala google docs
     */

    return (
        <div className={"flex flex-col"}>
            <input type="text"
                   value={fileName}
                   onChange={e => setFileName(e.target.value)}
                   className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                   placeholder="File Name"
            />
            <GroupSelector searchValue={searchValue} options={groupOptions}  setSearchValue={setSearchValue} setSelectedGroup={setSelectedGroup} selectedValue={selectedGroup}/>

            <UploadFileBtn/>

        </div>
    )

}