import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import {useAuth} from "../providers/AuthProvider";
import Modal from "../components/Modal";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUnlock} from "@fortawesome/free-solid-svg-icons";
import {NotFoundError} from "../errors/not-found.error";

export function Login() {
    const navigate = useNavigate()
    const {login} = useAuth();
    const [isModalOpen, setIsModalOpen] = useState(false)

    const handleSuccess = async (credentialResponse: CredentialResponse) => {
        login(credentialResponse)
            .then((user) => {
                console.log("existing user")
                setIsModalOpen(true)
                //navigate("/files")
            }).catch((error) => {
            if (error instanceof NotFoundError){
                navigate("/register")
            }
        })
    }


    const closeModal = () => {
        setIsModalOpen(false)
    };
    const modelContent = () => {
        return <div>
            <label className="block mb-2 ">Password
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
        </div>
    }
    return (
        <div className={"flex flex-col  items-center h-screen mt-40"}>
            <p className={"text-2xl"}>Welcome to FileSamurai</p>
            <div className={"flex-col"}>
                <p className={"text-2xl text-center p-2"}> Login</p>
                <GoogleLogin
                    onSuccess={async credentialResponse => {
                        await handleSuccess(credentialResponse)
                    }}
                    onError={() => {
                        console.log('Login Failed');
                    }}
                />
                <Modal onClose={closeModal} isOpen={isModalOpen} child={modelContent()}/>
            </div>

        </div>

    )

}