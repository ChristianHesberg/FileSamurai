import React from "react";
import {useAuth} from "../providers/AuthProvider";
import {Navigate} from "react-router-dom";

export function Home() {
    const {user, logout} = useAuth();


    return (
        <div>
            {user ? (
                <div>
                    <h2>Welcome, {user.name}</h2>
                    <img src={user.picture} alt="Profile"/>
                    <p>Email: {user.email}</p>
                    <button onClick={logout}>
                        Logout
                    </button>
                </div>
            ) : (<Navigate to={"/"}/>    )
            }
        </div>
    )

}