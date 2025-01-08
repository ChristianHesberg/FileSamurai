import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import {useAuth} from "../providers/AuthProvider";
import Modal from "../components/Modal";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUnlock} from "@fortawesome/free-solid-svg-icons";
import {NotFoundError} from "../errors/not-found.error";
import {useUseCases} from "../providers/UseCaseProvider";
import {useKey} from "../providers/KeyProvider";
import {PasswordInputModal} from "../components/PasswordInputModal";

export function Login() {
    const navigate = useNavigate()
    const { login, logout} = useAuth();

    const [isModalOpen, setIsModalOpen] = useState(false)

    const handleSuccess = async (credentialResponse: CredentialResponse) => {
        login(credentialResponse)
            .then(() => {
                setIsModalOpen(true)
            }).catch((error) => {
            if (error instanceof NotFoundError) {
                navigate("/register")
            }
        })
    }

    const closeModal = () => {
        logout()
        setIsModalOpen(false)
    };


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
                <Modal onClose={closeModal} isOpen={isModalOpen} child={<PasswordInputModal navigationRoute={"/files"}/>}/>
            </div>

        </div>

    )
}