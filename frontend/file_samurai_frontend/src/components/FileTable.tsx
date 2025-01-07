import TableOptionsBtn from "./TableOptionsBtn";
import React from "react";
import {faCloudArrowDown} from "@fortawesome/free-solid-svg-icons";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {useKey} from "../providers/KeyProvider";
import {useUseCases} from "../providers/UseCaseProvider";

const FileTable = () => {
    const { retrieveKey } = useKey();
    const { decryptFileUseCase } = useUseCases();
    const addMemberBtn = () => {
        return (
            <button
                className="block px-4 py-2 text-sm bg-lime-900 hover:bg-lime-700 w-full rounded"
                role="menuitem"
                onClick={() => {


                    const key = retrieveKey();
                    console.log(key);
                    const res = decryptFileUseCase.execute('2f486cba-c664-4689-acf3-6da89df99355', 'c0d4c997-6ee2-4b83-8146-8ac3880fb255', key!).then((v) => {
                        console.log(v.toString('utf8'));
                    })
                }}
            >
                Add member
            </button>
        )

    }
    const downloadBtn = () => {
        return (
            <button
                className="block px-4 py-2 text-sm bg-sky-600 hover:bg-sky-400 w-full rounded"
                role="menuitem"
                onClick={() => {
                }}
            >
                Download
                <FontAwesomeIcon icon={faCloudArrowDown} size={"xl"}/>
            </button>
        )

    }
    const deleteBtn = () => {
        return (
            <button
                className="block px-4 py-2 text-sm bg-red-900 hover:bg-red-800 w-full rounded"
                role="menuitem"
                onClick={() => {
                }}
            >
                Delete File
            </button>
        )
    }
    const buttons = () => {
        return [downloadBtn(),addMemberBtn(), deleteBtn()]
    }
    return (
        <div>
            <table className={"min-w-full text-left text-sm font-light text-surface dark:text-white"}>
                <thead className={"border-b border-neutral-200 font-medium dark:border-white/10"}>
                <tr>
                    <th scope={"col"} className={"px-6 py-4"}>
                        Name
                    </th>
                    <th scope={"col"} className={"px-6 py-4"}>

                    </th>
                </tr>
                </thead>
                <tbody>
                <tr className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600 group"}>
                    <td>
                        Very cool filler name!
                    </td>
                    <td className={"px-6 py-4"}>
                        <TableOptionsBtn children={[buttons()]}/>
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
    )
}

export default FileTable