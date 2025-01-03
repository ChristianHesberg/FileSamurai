import TableOptionsBtn from "./TableOptionsBtn";
import Modal from "./Modal";
import React, {useState} from "react";
import {Group} from "../models/Group";
import {AddMembersModal} from "./AddMembersModal";

interface GroupsTableProps {
    groups: Group[]
}

export const GroupsTable: React.FC<GroupsTableProps> = ({groups}) => {
    const [isModalOpen, setModalOpen] = useState(false);

    const openModal = () => setModalOpen(true);
    const closeModal = () => {
        setModalOpen(false)
    };
    const handleAddMemberClick = () => {

    }
    const handleDeleteGroupClick = () => {

    }

    const addMemberBtn = (key: string) => {
        return (
            <div  key={key}>
                <button
                    className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                    role="menuitem"
                    onClick={openModal}
                >
                    Add member
                </button>
                <Modal isOpen={isModalOpen} onClose={closeModal} child={<AddMembersModal/>}/>
            </div>
        )
    }

    const deleteGroupBtn = (key:string) => {
        return (
            <button
                key={key}
                className="block px-4 py-2 text-sm bg-red-900 hover:bg-red-800 w-full  rounded"
                role="menuitem"
                onClick={handleDeleteGroupClick}
            >
                Delete Group
            </button>
        )
    }
    const buttons = (groupId: string) => {
        return [addMemberBtn(groupId), deleteGroupBtn(groupId)]
    }

    return (

        <div>
            <table className={"min-w-full text-left text-sm font-light text-surface dark:text-white"}>
                <thead className={"border-b border-neutral-200 font-medium dark:border-white/10"}>
                <tr>
                    <th scope={"col"} className={"px-6 py-4"}>
                        Group name
                    </th>
                    <th scope={"col"} className={"px-6 py-4"}>
                        Member count
                    </th>
                    <th scope={"col"} className={"px-6 py-4"}>

                    </th>
                </tr>
                </thead>
                <tbody>
                {groups.map((group, index) => (
                    <tr
                        key={group.id}
                        className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600 group"}>
                        <td>
                            {group.name}
                        </td>
                        <td className={"whitespace-nowrap px-6 py-4"}>
                            N/A
                        </td>
                        <td>
                            <TableOptionsBtn children={buttons(group.id)}/>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}