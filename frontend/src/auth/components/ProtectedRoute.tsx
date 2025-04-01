import { FC, useContext } from "react"
import {Navigate, Outlet} from "react-router-dom"
import AuthContext, { AuthData } from "../auth-context"

const ProtectedRoute : FC = () => {
    const token = useContext<AuthData>(AuthContext)

    if(!token.token)
        return <Navigate to="/login"/>

    return <Outlet/>
}

export default ProtectedRoute