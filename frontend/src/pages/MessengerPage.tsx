import {FC, useContext} from "react";
import CreateUserModal from "../components/User/CreateUserModal/CreateUserModal.tsx";
import AuthContext, {AuthContextValue} from "../features/auth/AuthContext.ts";
import Messenger from "../components/Messenger/Messenger.tsx";

const MessengerPage: FC = () => {
    const authContext: AuthContextValue = useContext<AuthContextValue>(AuthContext)

    if (!authContext.data.user) {
        return <CreateUserModal setUser={authContext.setUser}/>
    }

    return <Messenger accessToken={authContext.data.accessToken!}/>
}

export default MessengerPage;