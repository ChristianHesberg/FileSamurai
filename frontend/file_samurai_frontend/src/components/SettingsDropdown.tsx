import React, {useState} from "react";
import {useAuth} from "../providers/AuthProvider";
import {Navigate, useNavigate} from "react-router-dom";

const SettingsDropdown: React.FC = () => {
    const [isOpen, setIsOpen] = useState(false);
    const {user, logout} = useAuth();
    const navigate = useNavigate()

    const handleLogoutClick = () => {
        logout()
        navigate("/login")
    }
    const toggleDropdown = () => {
        setIsOpen((prev) => !prev);
    };

    return (
        <div className="relative inline-block text-left">
            {/* Trigger Button */}
            <button
                onClick={toggleDropdown}
                className="inline-flex justify-center mb-2 w-full px-4 py-2 text-sm font-medium  border border-neutral-700 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 bg-neutral-900 hover:bg-neutral-700 "
            >
                Settings
                <svg
                    className="w-5 h-5 ml-2 -mr-1"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                    aria-hidden="true"
                >
                    <path
                        fillRule="evenodd"
                        d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 011.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                        clipRule="evenodd"
                    />
                </svg>
            </button>

            {/* Dropdown Menu */}
            {isOpen && (
                <div
                    className="absolute right-0 z-10 w-48 p-2 origin-top-right bg-neutral-900 border border-neutral-700 divide-y divide-gray-100 rounded-md shadow-lg focus:outline-none"
                    role="menu"
                >
                    <p className={"text-sm mb-1"}>Signed in as {user!.email}</p>
                    <div className="py-1" role="none">
                        <button
                            className="block px-4 py-2 text-sm bg-neutral-900 hover:bg-neutral-700 w-full border border-neutral-700 rounded"
                            role="menuitem"
                            onClick={ handleLogoutClick}
                        >
                            Logout
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default SettingsDropdown;
