const FileTable = () => {

    return (
        <div>
            <table className={"min-w-full text-left text-sm font-light text-surface dark:text-white"}>
                <thead className={"border-b border-neutral-200 font-medium dark:border-white/10"}>
                <tr>
                    <th scope={"col"} className={"px-6 py-4"}>
                        #
                    </th>
                    <th  scope={"col"} className={"px-6 py-4"}>
                        Name
                    </th>
                </tr>
                </thead>
                <tbody>
                <tr className={"border-b border-neutral-200 transition duration-300 ease-in-out hover:bg-neutral-100 dark:border-white/10 dark:hover:bg-neutral-600"}>
                    <td>
                        <input type={"checkbox"} className={"whitespace-nowrap px-6 py-4"}  />
                    </td>
                    <td className={"whitespace-nowrap px-6 py-4"}>
                        Very cool file name!
                    </td>
                </tr>
                </tbody>
            </table>
        </div>
    )
}

export default FileTable