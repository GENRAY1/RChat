import axios, {AxiosResponse, HttpStatusCode} from "axios";
import React, {FC, useCallback, useEffect, useMemo, useState} from "react";
import AuthContext from "../AuthContext.ts";
import {AuthData} from "../AuthData.ts";
import {getString} from "../../../shared/utils/local-storage-utils.ts";
import {LOCAL_STORAGE_KEYS} from "../../../shared/constants/local-storage-const.ts";
import {AccountService} from "../../../api/account-service/AccountService.ts";
import {CurrentAccount} from "../../../models/account/CurrentAccount.ts";
import {User} from "../../../models/user/User.ts";
import {apiClient} from "../../../api/api-client.ts";

type AuthProviderProps = {
    children: React.ReactNode
}

const AuthProvider : FC<AuthProviderProps> = ({ children }) => {
    const [authData, setAuthData] = useState<AuthData>({
        accessToken: getString(LOCAL_STORAGE_KEYS.accessToken)
    });

    const login = useCallback((accessToken: string): void => {
        setAuthData(prev => ({...prev, accessToken}));
        localStorage.setItem(LOCAL_STORAGE_KEYS.accessToken, accessToken);
    }, []);

    const logout = useCallback(() => {
        setAuthData({});
        localStorage.removeItem(LOCAL_STORAGE_KEYS.accessToken);
    }, []);

    const setUser = useCallback((user: User): void => {
        setAuthData(prev => ({...prev, user}));
    }, []);

    useEffect(() => {
        if (authData.accessToken) {
            apiClient.defaults.headers.common["Authorization"] = "Bearer " + authData.accessToken;
            const fetchCurrentAccount = async () => {
                try {
                    const accountResponse : AxiosResponse<CurrentAccount> = await AccountService.getCurrent();

                    setAuthData(prev => ({
                        ...prev,
                        user: accountResponse.data.user,
                        accountId: accountResponse.data.id,
                        accountRole: accountResponse.data.role,
                    }));
                }catch (error){
                    if(axios.isAxiosError(error) && error.status === HttpStatusCode.Unauthorized){
                        logout()
                    }
                }

            }

            fetchCurrentAccount();
        } else {
            delete apiClient.defaults.headers.common["Authorization"];
        }
    }, [authData.accessToken]);
    

    const contextValue = useMemo(
        () => ({
            data: authData,
            isAuthenticated: !!authData.accessToken,
            login: login,
            logout: logout,
            setUser : setUser,
        }), [authData, login, logout, setUser]);

    return (
        <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
    );
};

export default AuthProvider;