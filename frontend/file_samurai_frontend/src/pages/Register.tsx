import React, {useState} from "react";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faKey} from "@fortawesome/free-solid-svg-icons";
import {useAuth} from "../providers/AuthProvider";
import {useNavigate} from "react-router-dom";

export function Register() {
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState(" ")
    const [showPassword, setShowPassword] = useState(false);
    const {user, register, initSecret} = useAuth()
    const navigate = useNavigate()
    const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
        if (confirmPassword && e.target.value !== confirmPassword) {
            setError("Passwords do not match.");
        } else {
            setError("");
        }
    };
    const handleConfirmPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setConfirmPassword(e.target.value);
        if (password && e.target.value !== password) {
            setError("Passwords do not match.");
        } else {
            setError("");
        }
    };
    const handleToggleShowPassword = () => {
        setShowPassword(!showPassword);
    };
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!error && password && confirmPassword) {
            await register(user!.email, password)
                .then(() => {
                    navigate("/files")
                    initSecret(password)
                })
                .catch((e) => console.log(e))
        }
    };

    return (
        <div className={"flex flex-col justify-center items-center"}>
            <h1 className={"text-2xl"}>Your first time logging in!</h1>
            <h2 className={"text-xl"}>Please enter your secret key. You will need this to unlock all your files.</h2>
            <h2 className={"text-lg"}>Do not forget it</h2>

            <form className={"flex flex-col space-y-2 w-1/2"} onSubmit={handleSubmit}>
                <div className={"relative"}>
                    <button
                        type="button"
                        onClick={handleToggleShowPassword}
                        className="absolute inset-y-0 right-0 px-3 text-sm text-gray-500 hover:text-gray-700 focus:outline-none"
                    >
                        {showPassword ? "Hide" : "Show"}
                    </button>
                    <input type={showPassword ? "text" : "password"}
                           className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring flex h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                           placeholder="very secret password"
                           id="password"
                           value={password}
                           onChange={handlePasswordChange}

                    />
                </div>
                <div className={"relative"}>
                    <button
                        type="button"
                        onClick={handleToggleShowPassword}
                        className="absolute inset-y-0 right-0 px-3 text-sm text-gray-500 hover:text-gray-700 focus:outline-none"
                    >
                        {showPassword ? "Hide" : "Show"}
                    </button>

                    <input type={showPassword ? "text" : "password"}
                           className="border-input border-neutral-700 bg-neutral-800 ring-offset-background placeholder:text-muted-foreground
           focus-visible:ring-ring h-10 w-full rounded-md border px-3 py-2  focus-visible:outline-none
            focus-visible:ring-2 focus-visible:ring-offset-2"
                           placeholder="very secret password again"
                           id="confirm-password"
                           value={confirmPassword}
                           onChange={handleConfirmPasswordChange}
                    />
                </div>
                {error && (
                    <p className="mb-4 text-sm text-red-500">
                        {error}
                    </p>
                )}
                <button
                    type={"submit"}
                    disabled={!!error}
                    className={` mb-2 w-full px-4 py-2 text-sm font-medium border border-neutral-700 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 bg-neutral-900  ${error ? "bg-red-950 cursor-not-allowed" : "hover:bg-neutral-700"} `}>
                    Set key
                    <FontAwesomeIcon icon={faKey}/>
                </button>
            </form>

        </div>
    )
}