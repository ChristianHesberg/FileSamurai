import React, {useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBars} from "@fortawesome/free-solid-svg-icons/faBars";

interface TableOptionsBtnParams {
    children: React.ReactNode[]
}

const TableOptionsBtn: React.FC<TableOptionsBtnParams> = ({children}) => {
    const [isOpen, setIsOpen] = useState(false);


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
                        {children.map((button, index) => (
                            <div key={index}>
                                {button}
                            </div>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
}
export default TableOptionsBtn