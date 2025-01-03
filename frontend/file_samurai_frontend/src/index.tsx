import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import {GoogleOAuthProvider} from "@react-oauth/google";
import {BrowserRouter} from "react-router";
import {AuthProvider} from "./providers/AuthProvider";
import {UseCaseProvider} from "./providers/UseCaseProvider";

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <React.StrictMode>
        <GoogleOAuthProvider clientId="503035586312-ujnij8557gd7nga1lbjsvi56vi98iubb.apps.googleusercontent.com">
            <BrowserRouter>
                <AuthProvider>
                    <UseCaseProvider>
                        <App/>
                    </UseCaseProvider>
                </AuthProvider>
            </BrowserRouter>
        </GoogleOAuthProvider>
    </React.StrictMode>
);

