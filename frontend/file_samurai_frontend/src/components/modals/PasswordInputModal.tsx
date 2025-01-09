import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUnlock} from "@fortawesome/free-solid-svg-icons";
import React, {useState} from "react";
import {useUseCases} from "../../providers/UseCaseProvider";
import {useKey} from "../../providers/KeyProvider";
import {useAuth} from "../../providers/AuthProvider";
import {NavigateFunction} from "react-router/dist/production";
import {useNavigate} from "react-router-dom";

interface PasswordInputModalProps {
    navigationRoute?: string
}

export const PasswordInputModal: React.FC<PasswordInputModalProps> = ({navigationRoute}) => {
    const [error, setError] = useState<string>("")
    const [password, setPassword] = useState<string>("")
    const {validatePasswordHashUseCase, deriveEncryptionKeyUseCase} = useUseCases()
    const {storeKey} = useKey()
    const {user} = useAuth()
    const navigate = useNavigate()

    function handlePasswordSubmit(event: React.FormEvent) {
        event.preventDefault();
        if (password.length === 0) return
        validatePasswordHashUseCase.execute(password, user?.email!)
            .then((validPassword) => {
                if (!validPassword) {
                    setError("Wrong password")
                    return
                }
                deriveEncryptionKeyUseCase.execute(password, user?.userId!, user?.email!)
                    .then(key => {
                        storeKey(key)
                        if (navigationRoute) {
                            navigate(navigationRoute)
                        }
                    })
            })
            .catch(() => setError("Incorrect password"))
    }

    return <div>
        <label className="block mb-2 ">Password
            <form className="flex" onSubmit={handlePasswordSubmit}>
                <input id={"passwordInput"}
                       type="password"
                       className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-l-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                       placeholder="very secret password"
                       value={password}
                       onChange={e => setPassword(e.target.value)}

                />
                <button className="bg-lime-900 hover:bg-lime-700 rounded-r-md p-3"
                        type="submit"
                >
                    <FontAwesomeIcon icon={faUnlock}/>
                </button>
            </form>
            {error && (
                <p className={"text-red-500"}>{error}</p>
            )}
        </label>
    </div>
}