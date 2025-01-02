import React, {createContext, useContext, useState, ReactNode, useEffect, Dispatch, SetStateAction} from 'react';
import {CredentialResponse, googleLogout} from '@react-oauth/google';
import {jwtDecode} from "jwt-decode";
import {
    GetUserByEmailOrRegisterUseCaseFactory
} from "../use-cases/factories/get-user-by-email-or-register.use-case.factory";
import {User} from "../models/user.model";
import {GetUserByEmailUseCaseFactory} from "../use-cases/factories/get-user-by-email.use-case.factory";

// Define the user type based on Google's JWT payload
interface GoogleUser {
    name: string;
    email: string;
    picture: string;
    userId?: string;

    [key: string]: any; // For additional properties
}

// Define the context value type
interface AuthContextType {
    user: GoogleUser | null;
    isInitializing: boolean;
    login: (credentialResponse: CredentialResponse) => Promise<User>;
    logout: () => void;
    secret: string;
    initSecret: (string:string)=> void;
}

// Create the context
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// AuthProvider Props
interface AuthProviderProps {
    children: ReactNode;
}

// AuthProvider component
export const AuthProvider: React.FC<AuthProviderProps> = ({children}) => {
    const [user, setUser] = useState<GoogleUser | null>(null);
    const [isInitializing, setIsInitializing] = useState<boolean>(true);
    const [secret, setSecret] = useState<string>("");

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        const storedPassword = localStorage.getItem('password')
        if (storedUser && storedPassword) {
            setUser(JSON.parse(storedUser));
            setSecret(storedPassword)
        }
        setIsInitializing(false)
    }, []);

    const login = async (credentialResponse: CredentialResponse): Promise<User> => {
        const credentials = credentialResponse.credential!
        const decoded: GoogleUser = jwtDecode(credentials);
        const useCase = GetUserByEmailUseCaseFactory.create();
        localStorage.setItem('user', JSON.stringify(decoded));
        localStorage.setItem("jwtToken", credentials)
        setUser(decoded);
        const user = await useCase.execute(decoded.email);
        decoded.userId = user.id;
        return user
    };

    const logout = () => {
        googleLogout();
        setUser(null);
        localStorage.removeItem('user');
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('password');
    };

    const initSecret = (pass:string) => {
        setSecret(pass)
        localStorage.setItem("password",pass)
    }
    return (
        <AuthContext.Provider value={{user, login, logout, isInitializing, secret, initSecret}}>
            {children}
        </AuthContext.Provider>
    );
};

// Custom hook for accessing the context
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
