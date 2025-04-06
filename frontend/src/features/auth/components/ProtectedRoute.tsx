import { FC, useContext } from "react"
import {Navigate, Outlet} from "react-router-dom"
import AuthContext, {AuthContextValue} from "../AuthContext.ts"

const ProtectedRoute : FC = () => {
    const authContext : AuthContextValue = useContext<AuthContextValue>(AuthContext)

    if(!authContext.isAuthenticated)
        return <Navigate to="/login"/>

    return <Outlet/>
}

export default ProtectedRoute