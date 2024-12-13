import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBars} from "@fortawesome/free-solid-svg-icons/faBars";
import getGroups from "../templateData/GroupData"
import GroupOptionsDropdown from "./GroupOptionsDropdown";

export function GroupsTable() {
    const allGroups = getGroups()
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
                {allGroups.map(group => (
                    <tr className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600 group"}>
                        <td>
                            {group.name}
                        </td>
                        <td className={"whitespace-nowrap px-6 py-4"}>
                            {group.members.length} members
                        </td>
                        <td>
                            <GroupOptionsDropdown/>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}