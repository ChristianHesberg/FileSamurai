import React, {ReactNode} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPaperPlane} from "@fortawesome/free-solid-svg-icons";

interface ModalProps {
    isOpen: boolean;
    onClose: () => void;
    child: ReactNode;
}

const Modal: React.FC<ModalProps> = ({isOpen, onClose, child}) => {
    if (!isOpen) return null;
    return (
        <div
            className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50"
            onClick={onClose}
        >
            <div
                className="bg-neutral-800 rounded-lg shadow-lg p-6 w-full max-w-lg"
                onClick={(e) => e.stopPropagation()} // Prevent closing when clicking inside the modal
            >
                {child}

            </div>
        </div>
    )
}
export default Modal