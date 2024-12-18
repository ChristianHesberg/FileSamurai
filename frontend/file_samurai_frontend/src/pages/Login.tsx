import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import React from "react";
import {useNavigate} from "react-router-dom";
import {useAuth} from "../providers/AuthProvider";

export function Login() {
    const navigate = useNavigate()
    const {login} = useAuth();

    const handleSuccess = async (credentialResponse: CredentialResponse) => {
        login(credentialResponse)
            .then(() => navigate("/files"))
            .catch(e => console.log("login error: " + e))

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
            </div>

        </div>

    )

}