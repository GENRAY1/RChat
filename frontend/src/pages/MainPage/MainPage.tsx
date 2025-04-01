import {FC} from "react";
import styles from "./MainPage.module.css"
import Sidebar from "../../components/Sidebar/Sidebar.tsx";
import ChatPanel from "../../components/ChatPanel/ChatPanel.tsx";
import ChatArea from "../../components/ChatArea/ChatArea.tsx";

const MainPage: FC = () => {

    return (
        <div className={styles.container}>
            <Sidebar></Sidebar>
            <ChatPanel></ChatPanel>
            <ChatArea></ChatArea>
        </div>
    )
}

export default MainPage;