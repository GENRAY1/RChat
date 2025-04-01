import {FC} from 'react';
import styles from './UserChatItem.module.css'
import {ChatType} from "../../../models/chat/ChatType.ts";
import NameAvatar from "../../../shared/ui/NameAvatar/NameAvatar.tsx";
import GroupChatIcon from "../../../shared/component-icons/GroupChatIcon.tsx";
import {getDateOnly, getTimeOnly, isToday} from "../../../shared/utils/date-utils.ts";
import {Message} from "../../../models/message/Message.ts";

export interface ChatItemProps {
    chatName: string,
    chatType: ChatType,
    isLastMessageOutgoing: boolean,
    lastMessage: Message,
}

const UserChatItem: FC<ChatItemProps> = ({lastMessage, isLastMessageOutgoing, chatName, chatType}) => {

    const lastMessageTimestampStr: string = isToday(lastMessage.createdAt)
        ? getTimeOnly(lastMessage.createdAt)
        : getDateOnly(lastMessage.createdAt)

    const lastMessageSender: string = isLastMessageOutgoing
        ? "You"
        : lastMessage.sender.firstName

    return (
        <div className={styles.userChatItem}>
            <NameAvatar name={chatName} size={58} fontSize={16}/>
            <div className={styles.chatContent}>
                <div className={styles.chatHeader}>
                    <div className={styles.chatTitle}>
                        {chatType === ChatType.Group && <GroupChatIcon className={styles.chatIcon}></GroupChatIcon>}
                        <span className={styles.chatName}>{chatName}</span>
                    </div>
                    <span className={styles.chatTimestamp}>{lastMessageTimestampStr}</span>
                </div>

                <div className={styles.chatBody}>
                    {chatType === ChatType.Group && <div className={styles.lastMessageSender}>{lastMessageSender}:</div>}
                    <div className={styles.lastMessageText}>{lastMessage.text}</div>
                </div>
            </div>
        </div>
    );
};

export default UserChatItem;