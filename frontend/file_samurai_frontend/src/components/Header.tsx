import SettingsDropdown from "./SettingsDropdown";
import React from "react";
import {NavLink} from "react-router-dom";

function Header() {
    return (
        <nav className={"text-3xl flex justify-between mt-5"}>
            <div className={"w-full"}>
                <ul className={"flex flex-row justify-center space-x-10"}>
                    <li><NavLink to="/files"
                                 className={({isActive}) => `${isActive ? "text-sky-400" : ""}`}>Files</NavLink></li>
                    <li><NavLink to="/groups"
                                 className={({isActive}) => `${isActive ? "text-sky-400" : ""}`}>Groups </NavLink></li>

                </ul>
            </div>
            <SettingsDropdown/>
        </nav>
    )
}


export default Header