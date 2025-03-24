import { FC, useContext } from "react"
import {Navigate, Outlet} from "react-router-dom"
import AuthContext, { Auth } from "../auth-context"

const ProtectedRoute : FC = () => {
    const token = useContext<Auth>(AuthContext)

    if(!token.token)
        return <Navigate to="/login"/>

    return <Outlet/>
}

export default ProtectedRoute