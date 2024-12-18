import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import React from "react";
import {useNavigate} from "react-router-dom";
import {useAuth} from "../providers/AuthProvider";

export function Login() {
    const navigate = useNavigate()
    const {login} = useAuth();

    const handleSuccess = (credentialResponse: CredentialResponse) => {
        login(credentialResponse)
        console.log(credentialResponse);
        navigate("/files")
    }

    return (
        <div className={"flex flex-col  items-center h-screen mt-40"}>
            <p className={"text-2xl"}>Welcome to FileSamurai</p>
            <div className={"flex-col"}>
                <p className={"text-2xl text-center p-2"}> Login</p>
                <GoogleLogin
                    onSuccess={credentialResponse => {
                        handleSuccess(credentialResponse)
                    }}
                    onError={() => {
                        console.log('Login Failed');
                    }}
                />
            </div>

        </div>

    )

}