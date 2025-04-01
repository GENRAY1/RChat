import {createContext} from "react";
import {AuthUser} from "../models/user/AuthUser.ts";

export type AuthData = {
    token: string | null,
    user: AuthUser | null
    login: (token:string) => void,
    logout: () => void,
}

const AuthContext = createContext<AuthData>({
    token: null,
    user: null,
    login: () : void => {
        throw new Error("AuthProvider not initialized");
    },
    logout: () : void => {
        throw new Error("AuthProvider not initialized");
    }
});

export default AuthContext;