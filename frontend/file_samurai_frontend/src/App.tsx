import React, {useState} from 'react';
import logo from './logo.svg';
import './App.css';
import {GoogleLogin, googleLogout, GoogleOAuthProvider} from '@react-oauth/google';
import {useAuth} from "./providers/AuthProvider";
import {Login} from "./pages/Login";
import {Route, Routes} from "react-router";
import {Home} from "./pages/Home";

function App() {
    return (
        <div>
            <div>
                <h1>Welcome to the FileSamurai</h1>
                <Routes>
                    <Route path={"/home"} element={<Home/>}/>
                    <Route path={"/"} element={<Login/>}/>
                </Routes>
            </div>
        </div>
    );
}

export default App;
