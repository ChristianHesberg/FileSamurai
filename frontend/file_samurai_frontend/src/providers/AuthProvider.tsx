import React, {createContext, useContext, useState, ReactNode, useEffect} from 'react';
import {CredentialResponse, googleLogout} from '@react-oauth/google';
import {jwtDecode} from "jwt-decode";
import {
    GetUserByEmailOrRegisterUseCaseFactory
} from "../use-cases/factories/get-user-by-email-or-register.use-case.factory";

// Define the user type based on Google's JWT payload
interface User {
    name: string;
    email: string;
    picture: string;
    userId?: string;

    [key: string]: any; // For additional properties
}

// Define the context value type
interface AuthContextType {
    user: User | null;
    isInitializing: boolean;
    login: (credentialResponse: CredentialResponse) => Promise<void>;
    logout: () => void;
}

// Create the context
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// AuthProvider Props
interface AuthProviderProps {
    children: ReactNode;
}

// AuthProvider component
export const AuthProvider: React.FC<AuthProviderProps> = ({children}) => {
    const [user, setUser] = useState<User | null>(null);
    const [isInitializing, setIsInitializing] = useState<boolean>(true);

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        if (storedUser) setUser(JSON.parse(storedUser));
        setIsInitializing(false)
    }, []);

    const login = async (credentialResponse: CredentialResponse) => {
        const decoded: User = jwtDecode(credentialResponse.credential!);
        const useCase = GetUserByEmailOrRegisterUseCaseFactory.create();
        const user = await useCase.execute(decoded.email);
        decoded.userId = user.id;
        setUser(decoded);
        localStorage.setItem('user', JSON.stringify(decoded));
    };

    const logout = () => {
        googleLogout();
        setUser(null);
        localStorage.removeItem('user');
    };

    return (
        <AuthContext.Provider value={{user, login, logout, isInitializing}}>
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
