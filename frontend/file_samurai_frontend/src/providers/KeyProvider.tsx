import React, { createContext, useContext, useState, ReactNode } from 'react';

interface KeyContextType {
    storeKey: (key: CryptoKey) => void;
    retrieveKey: () => CryptoKey | undefined;
}

const KeyContext = createContext<KeyContextType>({
    storeKey: () => {},
    retrieveKey: () => undefined,
});

interface KeyProviderProps {
    children: ReactNode;
}

export const KeyProvider: React.FC<KeyProviderProps> = ({ children }) => {
    const [key, setKey] = useState<CryptoKey | undefined>(undefined);

    const storeKey = (newKey: CryptoKey) => {
        setKey(newKey);
    };

    const retrieveKey = (): CryptoKey | undefined => {
        return key;
    };

    return (
        <KeyContext.Provider value={{ storeKey, retrieveKey }}>
            {children}
        </KeyContext.Provider>
    );
};

export const useKey = (): KeyContextType => {
    return useContext(KeyContext);
};