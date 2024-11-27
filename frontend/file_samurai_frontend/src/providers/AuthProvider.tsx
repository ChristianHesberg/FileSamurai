import React, {createContext, useContext, useState, ReactNode, useEffect} from 'react';
import {CredentialResponse, googleLogout} from '@react-oauth/google';
import { jwtDecode } from "jwt-decode";
// Define the user type based on Google's JWT payload
interface User {
    name: string;
    email: string;
    picture: string;
    [key: string]: any; // For additional properties
}

// Define the context value type
interface AuthContextType {
    user: User | null;
    login: (credentialResponse: CredentialResponse) => void;
    logout: () => void;
}

// Create the context
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// AuthProvider Props
interface AuthProviderProps {
    children: ReactNode;
}

// AuthProvider component
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        if (storedUser) setUser(JSON.parse(storedUser));
    }, []);

    const login = (credentialResponse: CredentialResponse) => {
        const decoded: User = jwtDecode(credentialResponse.credential!);
        setUser(decoded);
        localStorage.setItem('user', JSON.stringify(decoded));
        console.log('Logged in:', decoded);
    };

    const logout = () => {
        googleLogout();
        setUser(null);
        localStorage.removeItem('user');
        console.log('User logged out');
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
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
