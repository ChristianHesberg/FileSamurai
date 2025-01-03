import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPaperPlane} from "@fortawesome/free-solid-svg-icons";
import React, {useEffect} from "react";

export const AddMembersModal = () => {

    useEffect(() => {
        function f() {
            console.log("test filler")
        }
    }, [])


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