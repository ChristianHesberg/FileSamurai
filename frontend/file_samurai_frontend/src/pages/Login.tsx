import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import React from "react";
import {useNavigate, useNavigation} from "react-router-dom";
import {useAuth} from "../providers/AuthProvider";

export function Login() {
    const navigate = useNavigate()
    const {user, login, logout} = useAuth();

    const handleSuccess = (credentialResponse: CredentialResponse) => {
        login(credentialResponse)
        navigate("/home")
        console.log(credentialResponse);
    }

    return (
        <GoogleLogin
            onSuccess={credentialResponse => {
                handleSuccess(credentialResponse)
            }}
            onError={() => {
                console.log('Login Failed');
            }}
        />
    )

}