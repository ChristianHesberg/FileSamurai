import {Selector} from "../Selector";
import React, {useEffect, useState} from "react";
import {SelectionOption} from "../../models/selectionOption";
import {useUseCases} from "../../providers/UseCaseProvider";
import {FileOption} from "../../models/FileOption";
import {FileAccessDto} from "../../models/FileAccessDto";
import {useAuth} from "../../providers/AuthProvider";
import {useKey} from "../../providers/KeyProvider";

interface ShareFileModalProps {
    selectedFile: FileOption,
    onClose: () => void
}

export const ShareFileModal: React.FC<ShareFileModalProps> = ({selectedFile, onClose}) => {
    const {
        getUsersInGroupUseCase,
        getAllFileAccessUseCase,
        shareFileWithMultipleUsersUseCase,
        deleteUserFileAccessUseCase
    } = useUseCases()

    const [roleOptions, setRoleOptions] = useState<SelectionOption[]>([{
        value: "Editor",
        label: "Editor"
    }, {value: "Viewer", label: "Viewer"}])
    const [selectedRole, setSelectedRole] = useState<SelectionOption>(roleOptions[0])

    const [userOptions, setUserOptions] = useState<SelectionOption[]>([])
    const [selectedUser, setSelectedUser] = useState<SelectionOption | null>(null)
    const [userSearchValue, setUserSearchValue] = useState<string>("")

    const [existingAccesses, setExistingAccesses] = useState<FileAccessDto[]>([])
    const [idEmailPairs, setIdEmailPairs] = useState<Map<string, string>>(new Map<string, string>())
    useEffect(() => {
        createUserOptions()
    }, []);
    const {user} = useAuth()
    const {retrieveKey} = useKey()

    const createUserOptions = async () => {
        const [options, existingAccesses] = await Promise.all(
            [
                getUsersInGroupUseCase.execute(selectedFile.groupId)
                    .then(r => {
                        return r.map((u) => ({
                            value: u.id,
                            label: u.email,
                        }))
                    }),
                getAllFileAccessUseCase.execute(selectedFile.id)
            ]
        )
        const set = new Set(existingAccesses.map(o => o.userId))
        const filteredOptions = options.filter(x => !set.has(x.value))

        setUserOptions(filteredOptions)
        //construct a map to pair ids with email. This is jank but it works
        const map = new Map<string, string>()
        set.forEach(userId =>
            options.forEach(op => {

                map.set(op.value, op.label)

            })
        )
        setExistingAccesses(existingAccesses)
        setIdEmailPairs(map)

    }
    const handleRoleChange = (selectedRole: any) => {
        setSelectedRole(selectedRole)
    }

    const handleUserChange = (selected: any) => {
        setSelectedUser(selected)
    }
    const handleShareBtn = () => {
        const arr = selectedUser as Array<SelectionOption> | null
        if (!arr) return
        const ids = arr.map(x => x.value);
        const uid = user?.userId!

        shareFileWithMultipleUsersUseCase.execute(uid, ids, selectedFile.id, retrieveKey()!, selectedRole.value)
            .then(() =>
                onClose()
            )
            .catch(e => console.log(e))
    };

    const getEmailLabel = (userid: string) => {
        return idEmailPairs.get(userid)
    }

    const handleDelBtn = (fileId: string, userId: string) => {
        deleteUserFileAccessUseCase.execute(userId, fileId)
            .then(()=> setExistingAccesses(prevState => prevState.filter(fa => fa.userId !== userId)))

        return undefined;
    }

    return (
        <div className={"flex flex-col"}>
            <h1 className={"text-2xl text-center"}>Share this file!</h1>
            <div className={"grid grid-cols-[70%_30%] gap-2"}>
                <Selector searchValue={userSearchValue} options={userOptions} setSearchValue={setUserSearchValue}
                          onChange={handleUserChange} selectedValue={selectedUser} isMulti={true}/>
                <Selector searchValue={userSearchValue} setSearchValue={setUserSearchValue} options={roleOptions}
                          onChange={handleRoleChange} selectedValue={selectedRole}/>
            </div>

            {existingAccesses ?
                <div>
                    <h1>Shared with:</h1>
                    {existingAccesses.filter(ea => ea.userId !== user?.userId).map(fileAccess =>
                        <div className={"flex flex-row"}>
                            <p>{getEmailLabel(fileAccess.userId)} | {fileAccess.role}</p>
                            <button className={"bg-red-700 px-1.5 rounded ml-1"}
                                    onClick={() => handleDelBtn(fileAccess.fileId, fileAccess.userId)}
                            >X
                            </button>
                        </div>
                    )}
                </div>
                : <></>
            }

            <button className={"bg-neutral-900 p-3 m-2 rounded"}
                    onClick={handleShareBtn}
            >Share
            </button>
        </div>
    )
}