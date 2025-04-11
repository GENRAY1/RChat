import {FC} from 'react';
import NameAvatar from "../../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import {GlobalSearchItem} from "../../../../../models/global-search/GlobalSearchItem.ts";
import styles from "./GlobalSearchItem.module.css"
import GroupChatIcon from "../../../../../shared/component-icons/GroupChatIcon.tsx";
import {GlobalSearchType} from "../../../../../models/global-search/GlobalSearchType.ts";

interface GlobalSearchItemProps {
    item: GlobalSearchItem;
}
const GlobalSearchItemComponent:FC<GlobalSearchItemProps> = ({item}) => {
    return (
        <div className={styles.container}>
            <NameAvatar fontSize={15} size={48} className={styles.avatar} name={item.displayName}/>
            <div className={styles.content}>
                <div className={styles.contentHeader}>
                    {item.type === GlobalSearchType.Group && <GroupChatIcon className={styles.headerIcon}></GroupChatIcon>}
                    <span className={styles.chatName}>{item.displayName}</span>
                </div>

                <div className={styles.contentBody}>
                    {item.type === GlobalSearchType.Group && item.membersCount}
                    {item.type === GlobalSearchType.User && "@" + item.username}
                </div>
            </div>
        </div>
    );
};

export default GlobalSearchItemComponent;