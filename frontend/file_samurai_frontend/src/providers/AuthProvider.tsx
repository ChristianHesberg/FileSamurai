import React, {createContext, ReactNode, useContext, useEffect, useState} from 'react';
import {CredentialResponse, googleLogout} from '@react-oauth/google';
import {jwtDecode} from "jwt-decode";
import {User} from "../models/user.model";
import {useUseCases} from "./UseCaseProvider";

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
    register: (email: string, password: string) => Promise<User>
    secret: string;
    initSecret: (string: string) => void;
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
    const {getUserByEmailUseCase, registerUserUseCase,createUserKeyPairUseCase} = useUseCases()

    useEffect(() => {
        const googleUser = getLocalStorageUser()
        const storedPassword = localStorage.getItem('password')
        if (googleUser && storedPassword) {
            setUser(googleUser);
            setSecret(storedPassword)
        }
        setIsInitializing(false)
    }, []);

    const login = async (credentialResponse: CredentialResponse): Promise<User> => {
        const credentials = credentialResponse.credential!
        const decoded: GoogleUser = jwtDecode(credentials);
        localStorage.setItem("jwtToken", credentials)
        localStorage.setItem('user', JSON.stringify(decoded));
        setUser(decoded);
        return await getUserByEmailUseCase.execute(decoded.email).then(user => {
            decoded.userId = user.id;
            localStorage.setItem('user', JSON.stringify(decoded));
            return user
        })
    };

    const register = async (email: string, password: string) => {
        const user=  await registerUserUseCase.execute(email, password);
        let googleUser = getLocalStorageUser()
        if (!googleUser) return Promise.reject()
        await createUserKeyPairUseCase.execute(password,user.email,user.id)
        googleUser.userId = user.id
        setUser(googleUser)
        localStorage.setItem("user", JSON.stringify(googleUser))
        return user

    }

    const getLocalStorageUser = (): GoogleUser | null => {
        const storedUser = localStorage.getItem('user');
        if (!storedUser) return null
        return JSON.parse(storedUser)

    }
    const logout = () => {
        googleLogout();
        setUser(null);
        localStorage.removeItem('user');
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('password');
    };

    const initSecret = (pass: string) => {
        setSecret(pass)
        localStorage.setItem("password", pass)
    }
    return (
        <AuthContext.Provider value={{user, login, logout, isInitializing, secret, initSecret, register}}>
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
