import {FC, memo} from 'react';
import styles from './UserChatItem.module.css'
import {ChatType} from "../../../../../models/chat/ChatType.ts";
import NameAvatar from "../../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import GroupChatIcon from "../../../../../shared/component-icons/GroupChatIcon.tsx";
import {getDateOnly, getTimeOnly, isToday} from "../../../../../shared/utils/date-utils.ts";
import {UserChat} from "../../../../../models/chat/UserChat.ts";

export interface UserChatItemProps {
    chat: UserChat,
    isLastMessageOutgoing: boolean,
    isActive: boolean;
    onActivate: (chat: UserChat) => void;
}

function getChatTimestampStr(date: Date){
    if(isToday(date))
        return getTimeOnly(date)

    return getDateOnly(date)
}

const UserChatItem: FC<UserChatItemProps> = ({chat, isLastMessageOutgoing, isActive, onActivate}) => {
    const timestamp : Date = chat.latestMessage?.createdAt ?? chat.createdAt

    const message = chat.latestMessage
        ? <div className={styles.lastMessageText}>{chat.latestMessage.text}</div>
        : <div className={`${styles.lastMessageText} ${styles.primaryText}`}>Chat created</div>

    return (
        <div className={`${styles.userChatItem} ${isActive  ? styles.userChatItemActive : ""}` } onClick={()=> onActivate(chat)}>
            <NameAvatar name={chat.displayName} size={58} fontSize={16}/>
            <div className={styles.chatContent}>
                <div className={styles.chatHeader}>
                    <div className={styles.chatTitle}>
                        {chat.type === ChatType.Group && <GroupChatIcon className={styles.chatIcon}></GroupChatIcon>}
                        <span className={styles.chatName}>{chat.displayName}</span>
                    </div>
                    <span className={styles.chatTimestamp}>
                        {getChatTimestampStr(timestamp)}
                    </span>
                </div>

                <div className={styles.chatBody}>
                    {chat.type  === ChatType.Group &&
                        <div className={styles.primaryText}>
                            {isLastMessageOutgoing && chat.latestMessage ? "You" : chat.latestMessage?.sender.firstname}:
                        </div>}
                    {message}
                </div>
            </div>
        </div>
    );
};



export default memo(UserChatItem, (prev, next) => {
    return (
        prev.chat === next.chat &&
        prev.isLastMessageOutgoing === next.isLastMessageOutgoing &&
        prev.isActive === next.isActive
    );
});