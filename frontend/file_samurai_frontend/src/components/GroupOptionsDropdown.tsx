import React, {useState} from "react";
import {useAuth} from "../providers/AuthProvider";
import {useNavigate} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBars} from "@fortawesome/free-solid-svg-icons/faBars";
import Modal from "./Modal";

const GroupOptionsDropdown: React.FC = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [isModalOpen, setModalOpen] = useState(false);

    const openModal = () => setModalOpen(true);
    const closeModal = () => {
        setIsOpen(false)
        setModalOpen(false)
    };
    const handleAddMemberClick = () => {

    }
    const handleDeleteGroupClick = () => {

    }
    const toggleDropdown = () => {
        setIsOpen((prev) => !prev);
    };

    return (
        <div className="relative inline-block text-left">
            {/* Trigger Button */}
            <button className={"invisible group-hover:visible "} onClick={toggleDropdown}>
                <FontAwesomeIcon icon={faBars} size={"2x"}/>
            </button>

            {/* Dropdown Menu */}
            {isOpen && (
                <div
                    className="absolute -left-1/2 top-8 z-10 w-48 p-2 origin-top-right bg-neutral-900 border border-neutral-700 divide-y divide-gray-100 rounded-md shadow-lg focus:outline-none"
                    role="menu"
                >
                    <div className="flex-col space-y-3" role="none">
                        <button
                            className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                            role="menuitem"
                            onClick={openModal}
                        >
                            Add member
                        </button>
                        <Modal isOpen={isModalOpen} onClose={closeModal}/>

                        <button
                            className="block px-4 py-2 text-sm bg-red-900 hover:bg-red-800 w-full  rounded"
                            role="menuitem"
                            onClick={handleDeleteGroupClick}
                        >
                            Delete Group
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
}
export default GroupOptionsDropdown