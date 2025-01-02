import getGroups from "../templateData/GroupData"
import TableOptionsBtn from "./TableOptionsBtn";
import Modal from "./Modal";
import React, {FC, useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPaperPlane} from "@fortawesome/free-solid-svg-icons";
import {Group} from "../models/Group";

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

    const modelContent = () => {
        return <div>
            <label className="block mb-2 ">Invite Email
                <div className="flex">
                    <input type="text"
                           className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-l-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                           placeholder="Newguy@gmail.com"

                    />
                    <button className="bg-lime-900 hover:bg-lime-700 rounded-r-md p-3"
                            type="button">
                        <FontAwesomeIcon icon={faPaperPlane}/>
                    </button>
                </div>
            </label>

            //TODO: TABLE OF ALL MEMBERS WITH KICK BUTTON
        </div>
    }

    const addMemberBtn = () => {
        return (
            <div>
                <button
                    className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                    role="menuitem"
                    onClick={openModal}
                >
                    Add member
                </button>
                <Modal isOpen={isModalOpen} onClose={closeModal} child={modelContent()}/>
            </div>
        )


    }

    const deleteGroupBtn = () => {
        return (
            <button
                className="block px-4 py-2 text-sm bg-red-900 hover:bg-red-800 w-full  rounded"
                role="menuitem"
                onClick={handleDeleteGroupClick}
            >
                Delete Group
            </button>
        )
    }
    const buttons = () => {
        return [addMemberBtn(), deleteGroupBtn()]
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
                {groups.map(group => (
                    <tr className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600 group"}>
                        <td>
                            {group.name}
                        </td>
                        <td className={"whitespace-nowrap px-6 py-4"}>
                            N/A
                        </td>
                        <td>
                            <TableOptionsBtn children={buttons()}/>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}