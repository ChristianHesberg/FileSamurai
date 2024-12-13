import React from 'react';
import './App.css';
import {Login} from "./pages/Login";
import {Route, Routes} from "react-router";
import {Files} from "./pages/Files";
import ProtectedRoute from "./providers/ProtectedRoute";
import {Groups} from "./pages/Groups";
import {Navigate, useLocation} from "react-router-dom";
import Header from "./components/Header";
import {useAuth} from "./providers/AuthProvider";

function App() {
    const location = useLocation()
    const hideHeader = location.pathname === "/login";
    const hideHeader2 = location.pathname === "/";
    const {user} = useAuth()
    return (
        <div className={"bg-neutral-950 text-gray-300 flex h-screen"}>

            <div className={"container mx-auto"}>
                {user && <Header/>}
                <Routes>
                    <Route path="/" element={
                        !user ?
                            <Navigate to="/login"/> : <Navigate to={"files"}/>
                    }/>

                    <Route path={"/Login"} element={<Login/>}/>

                    <Route path={"/files"} element={
                        <ProtectedRoute>
                            <Files/>
                        </ProtectedRoute>}/>
                    <Route path={"/groups"} element={
                        <ProtectedRoute>
                            <Groups/>
                        </ProtectedRoute>}/>

                </Routes>
            </div>
        </div>
    );
}

export default App;
