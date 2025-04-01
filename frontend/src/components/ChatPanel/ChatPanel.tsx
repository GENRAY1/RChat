import {useState} from "react";
import {UserChat} from "../../models/chat/UserChat.ts";
import UserChatItem from "./UserChatItem/UserChatItem.tsx";
import styles from "./ChatPanel.module.css"
import {USER_CHATS} from "../../shared/stabs/user-chat-stubs.ts";

const ChatPanel = () => {
    const [chats, setChats] = useState<UserChat[]>(USER_CHATS);

    return (
        <div className={styles.userPanel}>
            <div className={styles.header}>
                <input type="text" placeholder="Search" className={styles.searchInput}/>
            </div>

            <div className={styles.content}>
                <div className="user-chats">
                    {chats.map((chat) => <UserChatItem
                        key={chat.id}
                        chatType={chat.type}
                        chatName={chat.displayName}
                        lastMessage={chat.lastMessage}
                        isLastMessageOutgoing={false}
                    />)}
                </div>
            </div>
        </div>
    );
};

export default ChatPanel;