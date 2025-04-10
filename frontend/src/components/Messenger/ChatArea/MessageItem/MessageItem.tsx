import {FC, memo} from 'react';
import styles from './MessageItem.module.css'
import NameAvatar from "../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import {getDateOnly, getTimeOnly, isToday} from "../../../../shared/utils/date-utils.ts";
import {Message} from "../../../../models/message/Message.ts";


export interface MessageProps {
    isOutgoing: boolean,
    message: Message
}

const MessageItem: FC<MessageProps> = ({ isOutgoing, message }) => {
    const senderName = message.sender.lastname
        ? `${message.sender.firstname} ${message.sender.lastname}`
        : message.sender.firstname;

    const messageTimestampStr = isToday(message.createdAt)
        ? getTimeOnly(message.createdAt)
        : `${getDateOnly(message.createdAt)} ${getTimeOnly(message.createdAt)}`

    const messageClasses = `${styles.messageItem} ${isOutgoing ? styles.outgoing : styles.incoming}`;
    return (
        <div className={messageClasses}>
            <NameAvatar name={senderName} size={36} fontSize={12} className={styles.avatar} />
            <div className={styles.content}>
                <div className={styles.info}>
                    <span className={styles.sender}>{senderName}</span>
                    <span className={styles.timestamp}>{messageTimestampStr}</span>
                </div>
                <div className={styles.bubble}>
                    <div>{message.text}</div>
                </div>
            </div>
        </div>
    );
};

export default memo(MessageItem);