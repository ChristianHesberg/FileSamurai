import React from "react";
import {Navigate} from "react-router-dom";
import {useAuth} from "./AuthProvider";
import {useKey} from "./KeyProvider";

interface ProtectedRouteProps {
    children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({children}) => {
    const {user, isInitializing} = useAuth();

    if (isInitializing) {
        return <div>Loading...</div>;
    }

    return user ? <>{children}</> : <Navigate to="/login"/>;
};

export default ProtectedRoute;
