import styles from "./ChatPanel.module.css"
import UserChatList from "./UserChatList/UserChatList.tsx";
import GlobalSearchOverlay from "./Search/GlobalSearchOverlay/GlobalSearchOverlay.tsx";
import GlobalSearch from "./Search/GlobalSearch/GlobalSearch.tsx";

const ChatPanel = () => {
    return (
        <div className={styles.userPanel}>
            <div className={styles.header}>
                <GlobalSearch/>
            </div>

            <div className={styles.content}>
                <GlobalSearchOverlay />
                <UserChatList />
            </div>
        </div>
    );
};

export default ChatPanel;