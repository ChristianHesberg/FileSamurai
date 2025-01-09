import React, {useState} from "react";
import {Navigate} from "react-router-dom";
import {useAuth} from "./AuthProvider";
import {useKey} from "./KeyProvider";
import Modal from "../components/Modal";
import {PasswordInputModal} from "../components/modals/PasswordInputModal";

interface KeyProtectedRouteProps {
    children: React.ReactNode;
}

const KeyProtectedRoute: React.FC<KeyProtectedRouteProps> = ({children}) => {
    //const [modalOpen, setModalOpen] = useState<boolean>(false)
    const {retrieveKey} = useKey()

    return retrieveKey() ? <>{children}</> :
        <Modal isOpen={true} onClose={() => {}} child={<PasswordInputModal/>}/>;
};

export default KeyProtectedRoute;
