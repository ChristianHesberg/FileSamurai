import React from 'react';
import './App.css';
import {Login} from "./pages/Login";
import {Route, Routes} from "react-router";
import {Home} from "./pages/Home";

function App() {
    return (
        <div className={"bg-neutral-950 text-gray-300 flex h-screen"}>
            <div className={"container mx-auto"}>
                <Routes>
                    <Route path={"/home"} element={<Home/>}/>
                    <Route path={"/"} element={<Login/>}/>
                </Routes>
            </div>
        </div>
    );
}

export default App;
