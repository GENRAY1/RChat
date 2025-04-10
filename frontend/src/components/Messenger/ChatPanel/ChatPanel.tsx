import styles from "./ChatPanel.module.css"
import UserChatList from "./UserChatList/UserChatList.tsx";

const ChatPanel = () => {
    return (
        <div className={styles.userPanel}>
            <div className={styles.header}>
                <input type="text" placeholder="Search" className={styles.searchInput}/>
            </div>

            <div className={styles.content}>
                <UserChatList/>
            </div>
        </div>
    );
};

export default ChatPanel;