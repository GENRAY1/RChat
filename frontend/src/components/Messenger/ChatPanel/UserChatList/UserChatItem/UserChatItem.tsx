import {FC, memo} from 'react';
import styles from './UserChatItem.module.css'
import {ChatType} from "../../../../../models/chat/ChatType.ts";
import NameAvatar from "../../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import GroupChatIcon from "../../../../../shared/component-icons/GroupChatIcon.tsx";
import {getDateOnly, getTimeOnly, isToday} from "../../../../../shared/utils/date-utils.ts";
import {UserChat} from "../../../../../models/chat/UserChat.ts";
import {getDisplayName} from "../../../../../features/chat-features.tsx";

export interface UserChatItemProps {
    chat: UserChat;
    authUserId: number;
    isSelected: boolean;
    selectDialog: (userChat: UserChat, displayName: string, info: string) => void;
}

function getChatTimestampStr(date: Date) : string {
    if (isToday(date))
        return getTimeOnly(date)

    return getDateOnly(date)
}

const UserChatItem: FC<UserChatItemProps> = ({chat, authUserId, isSelected, selectDialog}) => {
    const Message = chat.latestMessage
        ? <div className={styles.lastMessageText}>{chat.latestMessage.text}</div>
        : <div className={`${styles.lastMessageText} ${styles.primaryText}`}>Chat created</div>

    const isLastMessageOutgoing : boolean = chat.latestMessage
        ? authUserId === chat.latestMessage.sender.userId
        : false

    const timestamp : Date = chat.latestMessage?.createdAt ?? chat.createdAt

    const displayName:string = getDisplayName(chat, authUserId) ?? ""

    function handleSelectItem(){

        if(isSelected) return

        selectDialog(chat, displayName, "-")
    }

    return (
        <div className={`${styles.userChatItem} ${isSelected  ? styles.userChatItemActive : ""}` } onClick={handleSelectItem}>
            <NameAvatar name={displayName} size={58} fontSize={16}/>
            <div className={styles.chatContent}>
                <div className={styles.chatHeader}>
                    <div className={styles.chatTitle}>
                        {chat.type === ChatType.Group && <GroupChatIcon className={styles.chatIcon}></GroupChatIcon>}
                        <span className={styles.chatName}>{displayName}</span>
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
                    {Message}
                </div>
            </div>
        </div>
    );
};



export default memo(UserChatItem, (prev, next) => {
    return (
        prev.chat === next.chat &&
        prev.authUserId === next.authUserId &&
        prev.isSelected === next.isSelected
    );
});