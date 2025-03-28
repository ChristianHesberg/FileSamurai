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
import {Register} from "./pages/Register";
import {KeyProvider} from "./providers/KeyProvider";
import KeyProtectedRoute from "./providers/KeyProtectedRoute";

function App() {
    const {user} = useAuth()
    const location = useLocation();

    const hideHeaderOnRoutes = ["/login", "/", "/register"]
    const hideHeader = !hideHeaderOnRoutes.includes(location.pathname);


    return (
        <div className={"bg-neutral-950 text-gray-300 flex h-screen"}>

            <div className={"container mx-auto"}>
                {user && hideHeader && <Header/>}
                <Routes>
                    <Route path="/" element={
                        !user ?
                            <Navigate to="/login"/> : <Navigate to={"files"}/>
                    }/>

                    <Route path={"/Login"} element={<Login/>}/>

                    <Route path={"/files"} element={
                        <ProtectedRoute>
                            <KeyProtectedRoute>
                                <Files/>
                            </KeyProtectedRoute>
                        </ProtectedRoute>}/>
                    <Route path={"/groups"} element={
                        <ProtectedRoute>
                                <Groups/>
                        </ProtectedRoute>}/>
                    <Route path={"/register"} element={
                        <ProtectedRoute>
                            <Register/>
                        </ProtectedRoute>
                    }/>
                </Routes>
            </div>
        </div>
    );
}

export default App;
