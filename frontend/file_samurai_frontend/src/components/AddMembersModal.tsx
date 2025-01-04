import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPaperPlane} from "@fortawesome/free-solid-svg-icons";
import React, {FormEvent, FormEventHandler, useEffect, useState} from "react";
import {useUseCases} from "../providers/UseCaseProvider";
import {User} from "../models/user.model";
import {useAuth} from "../providers/AuthProvider";

interface AddMembersModalProps {
    groupId: string;
}

export const AddMembersModal: React.FC<AddMembersModalProps> = ({groupId}) => {
    const [users, setUsers] = useState<User[]>([])
    const {getUsersInGroup, addUserToGroupUseCase,} = useUseCases()
    const [email, setEmail] = useState<string>("")

    useEffect(() => {
        getUsersInGroup.execute(groupId)
            .then(r => setUsers(r))
            .catch(() => console.log("failed to get users in group"))
    }, []);

    const handleAddButton = (e: React.FormEvent) => {
        e.preventDefault()
        addUserToGroupUseCase.execute(email, groupId)
            .then((r) => setUsers((prevState) => [...prevState, r]))
            .catch(e => console.log(e))
    }

    return <div>
        <label className="block mb-2 ">Invite Email
            <form className="flex" onSubmit={handleAddButton}>
                <input type="text"
                       value={email}
                       onChange={e => setEmail(e.target.value)}
                       className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-l-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                       placeholder="Newguy@gmail.com"
                />
                <button className="bg-lime-900 hover:bg-lime-700 rounded-r-md p-3"
                        type="submit">
                    <FontAwesomeIcon icon={faPaperPlane}/>
                </button>
            </form>
        </label>

        {users.map((user, index) => (
            <p key={index}>{user.email}</p>
        ))}
    </div>
}