import {FC, useContext} from "react";
import styles from "./ChatPage.module.css"
import Sidebar from "../../components/Sidebar/Sidebar.tsx";
import ChatPanel from "../../components/ChatPanel/ChatPanel.tsx";
import ChatArea from "../../components/ChatArea/ChatArea.tsx";
import CreateUserModal from "../../components/User/CreateUserModal/CreateUserModal.tsx";
import AuthContext, {AuthContextValue} from "../../features/auth/AuthContext.ts";

const ChatPage: FC = () => {
    const authContext : AuthContextValue = useContext<AuthContextValue>(AuthContext)

    if (authContext.isAuthenticated && !authContext.data.user){
       return <CreateUserModal setUser={authContext.setUser}/>
    }

    return (
        <div className={styles.container}>
            <Sidebar></Sidebar>
            <ChatPanel></ChatPanel>
            <ChatArea></ChatArea>
        </div>
    )
}

export default ChatPage;