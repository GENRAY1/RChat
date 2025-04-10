import {FC, useEffect} from 'react';
import styles from "./Messenger.module.css";
import Sidebar from "./Sidebar/Sidebar.tsx";
import ChatPanel from "./ChatPanel/ChatPanel.tsx";
import ChatArea from "./ChatArea/ChatArea.tsx";
import useChatStore from "../../store/chat-store.ts";


interface Messenger {
    accessToken: string;
}

const Messenger: FC<Messenger> = ({accessToken}) => {
    const isConnected = useChatStore(store => store.isConnected)
    const startConnection = useChatStore(store => store.startConnection)
    const stopConnection = useChatStore(store => store.stopConnection)

    useEffect(() => {
        startConnection(accessToken);

        return () => {
            stopConnection();
        };
    }, [accessToken, startConnection, stopConnection]);


    return (
        <div className={styles.container}>
            {!isConnected && <div className={styles.connectionLost}>Connection lost</div>}
            <div className={styles.layout}>
                <Sidebar></Sidebar>
                <ChatPanel></ChatPanel>
                <ChatArea></ChatArea>
            </div>
        </div>
    );
};

export default Messenger;