import Modal from "./Modal";
import React, {useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUnlock} from "@fortawesome/free-solid-svg-icons";

const NewUserModal: React.FC = () => {
    const [isModalOpen, setIsModalOpen] = useState(false)

    const onModalClose = () => {
        setIsModalOpen(false)
        // read inputs post new user
    }

    const content = () => {
        return (
            <label className="block mb-2 ">WELCOME NEW USER! ENTER YOUR PASSWORD
                <div className="flex">
                    <input type="text"
                           className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-l-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                           placeholder="very secret password"

                    />
                    <button className="bg-lime-900 hover:bg-lime-700 rounded-r-md p-3"
                            type="button">
                        <FontAwesomeIcon icon={faUnlock}/>
                    </button>
                </div>
            </label>
        )
    }
    return (
        <Modal isOpen={isModalOpen} onClose={onModalClose} child={content()}/>
    )
}

// @ts-ignore
export default NewUserModal()