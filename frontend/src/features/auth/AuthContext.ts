import {createContext} from "react";
import {AuthData} from "./AuthData.ts";
import {User} from "../../models/user/User.ts";

export type AuthContextValue = {
    data: AuthData,
    isAuthenticated: boolean,
    login: (token:string) => void,
    logout: () => void,
    setUser: (data: User) => void,
}

const AuthContext = createContext<AuthContextValue>({
    data: {},
    isAuthenticated: false,
    login: () : void => {
        throw new Error("AuthProvider not initialized");
    },
    logout: () : void => {
        throw new Error("AuthProvider not initialized");
    },
    setUser: () : void => {
        throw new Error("AuthProvider not initialized");
    },
});

export default AuthContext;