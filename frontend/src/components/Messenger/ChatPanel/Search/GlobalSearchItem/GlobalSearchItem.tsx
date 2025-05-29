import {FC, memo} from 'react';
import NameAvatar from "../../../../../shared/ui/NameAvatar/NameAvatar.tsx";
import styles from "./GlobalSearchItem.module.css"
import GroupChatIcon from "../../../../../shared/component-icons/GroupChatIcon.tsx";
import {ChatType} from "../../../../../models/chat/ChatType.ts";
import {GlobalSearchItemType} from "../../../../../models/global-search/GlobalSearchItemType.ts";
import {GlobalSearchItem} from "../../../../../models/global-search/GlobalSearchItem.ts";

export interface GlobalSearchItemProps {
    item: GlobalSearchItem;
    isSelected: boolean;
    selectDialog: (searchType: GlobalSearchItem, displayName: string) => Promise<void>;
}

const GlobalSearchItemComponent:FC<GlobalSearchItemProps> = ({item, selectDialog, isSelected}) => {

    const displayName:string = item.itemType === GlobalSearchItemType.User
        ? `${item.firstname} ${item.lastname || ''}`.trim()
        : item.name

    const info : string = item.itemType === GlobalSearchItemType.User
        ?`@${item.username}`
        : `${item.memberCount} members`;

    const isGroup = item.itemType === GlobalSearchItemType.Chat && item.type === ChatType.Group

    async function handleSelectItem (){

        if(isSelected) return

        await selectDialog(item, displayName)
    }

    return (
        <div className={styles.container} onClick={handleSelectItem}>
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

export default memo(GlobalSearchItemComponent);