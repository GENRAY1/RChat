import styles from "./ChatArea.module.css"
import {useState} from "react";
import {Message} from "../../models/message/Message.ts";
import {CHAT_MESSAGES} from "../../shared/stabs/message-stabs.ts";
import MessageItem from "./MessageItem/MessageItem.tsx";
import MessageInputArea from "./MessageInputArea/MessageInputArea.tsx";

const ChatArea = () => {
    const [messages, setMessages] = useState<Message[]>(CHAT_MESSAGES);

    return (
        <div className={styles.chatArea}>
            <div className={styles.header}>
                <div className={styles.chatName}>Название чата</div>
                <div className={styles.onlineInfo}>онлайн</div>
            </div>
            <div className={styles.content}>
                {messages.map((m) => <MessageItem key={m.id} message={m} isOutgoing={false}/>)}
            </div>
            <div className={styles.bottom}>
                <MessageInputArea/>
            </div>
        </div>
    );
};

export default ChatArea;