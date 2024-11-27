import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import {GoogleOAuthProvider} from "@react-oauth/google";
import {BrowserRouter} from "react-router";
import {AuthProvider} from "./providers/AuthProvider";

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <React.StrictMode>
        <GoogleOAuthProvider clientId="503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com">
            <BrowserRouter>
                <AuthProvider>
                    <App/>
                </AuthProvider>
            </BrowserRouter>
        </GoogleOAuthProvider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
