import axios from "axios";
import React, {FC, useEffect, useMemo, useState } from "react";
import AuthContext from "../auth-context.ts";
import {AuthUser} from "../../models/user/AuthUser.ts";
import {UserService} from "../../api/user-service/UserService.ts";

type AuthProviderProps = {
    children: React.ReactNode
}

const LOCAL_STORAGE = {
    user: 'user',
    accessToken: 'accessToken',
}

const AuthProvider : FC<AuthProviderProps> = ({ children }) => {

    const localAccessToken : string | null 
        = localStorage.getItem(LOCAL_STORAGE.accessToken)

    const localUserJson : string | null =
        localStorage.getItem(LOCAL_STORAGE.user);

    const localUser : AuthUser | null =  localUserJson
        ? JSON.parse(localUserJson)
        : null;

    const [token, setToken] = useState<string | null>(localAccessToken);
    const [user, setUser] = useState<AuthUser | null>(localUser);

    function updateUserData(data: AuthUser) {
        localStorage.setItem(LOCAL_STORAGE.user, JSON.stringify(data));
        setUser(data);
    }

    useEffect(() => {
        if (token) {
            axios.defaults.headers.common["Authorization"] = "Bearer " + token;
            localStorage.setItem(LOCAL_STORAGE.accessToken, token);

            if(!user){
                const fetchUser = async () => {
                    try {
                        const userData:AuthUser = await UserService.GetMe();
                        updateUserData(userData);
                    }catch (error){
                        console.error("Ошибка при загрузке пользователя:", error);
                    }
                }
                fetchUser();
            }

        } else {
            delete axios.defaults.headers.common["Authorization"];
            localStorage.removeItem(LOCAL_STORAGE.accessToken)
            localStorage.removeItem(LOCAL_STORAGE.user);
        }
    }, [token]);
    

    const contextValue = useMemo(
        () => ({
            token,
            user,
            setUser: updateUserData,
            login: (newToken:string) => setToken(newToken),
            logout: () => setToken(null),
        }), [token, user]);

    return (
        <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
    );
};

export default AuthProvider;