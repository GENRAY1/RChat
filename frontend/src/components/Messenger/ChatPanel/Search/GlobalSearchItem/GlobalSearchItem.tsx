import {FC} from 'react';
import NameAvatar from "../../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import styles from "./GlobalSearchItem.module.css"
import GroupChatIcon from "../../../../../shared/component-icons/GroupChatIcon.tsx";
import {ChatSearchItem, isChatSearchItem} from "../../../../../models/global-search/ChatSearchItem.ts";
import {isUserSearchItem, UserSearchItem} from "../../../../../models/global-search/UserSearchItem.ts";
import {ChatType} from "../../../../../models/chat/ChatType.ts";

export interface GlobalSearchItemProps {
    item: ChatSearchItem | UserSearchItem
}

const GlobalSearchItemComponent:FC<GlobalSearchItemProps> = ({item}) => {

    const displayName:string = isUserSearchItem(item)
        ? `${item.firstname} ${item.lastname || ''}`.trim()
        : item.name

    const info : string = isUserSearchItem(item)
        ?`@${item.username}`
        : `${item.memberCount} members`;

    const isGroup = isChatSearchItem(item) && item.type === ChatType.Group

    return (
        <div className={styles.container}>
            <NameAvatar fontSize={15} size={48} className={styles.avatar} name={displayName}/>
            <div className={styles.content}>
                <div className={styles.contentHeader}>
                    {isGroup && <GroupChatIcon className={styles.headerIcon}></GroupChatIcon>}
                    <span className={styles.chatName}>{displayName}</span>
                </div>

                <div className={styles.contentBody}>{info}</div>
            </div>
        </div>
    );
};

export default GlobalSearchItemComponent;