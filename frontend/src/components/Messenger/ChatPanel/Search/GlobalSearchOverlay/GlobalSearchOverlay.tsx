import styles from './GlobalSearchOverlay.module.css'
import useGlobalSearchState from "../../../../../store/global-search-store/global-search-store.ts";
import GlobalSearchItemComponent from "../GlobalSearchItem/GlobalSearchItem.tsx";
import {GlobalSearchItem} from "../../../../../models/global-search/GlobalSearchItem.ts";
import useChatStore from "../../../../../store/chat-store/chat-store.ts";
import {GlobalSearchItemType} from "../../../../../models/global-search/GlobalSearchItemType.ts";


const GlobalSearchOverlay = () => {
    const isActive = useGlobalSearchState(s => s.isOverlayActive)
    const informMessage = useGlobalSearchState(s => s.message)
    const isLoading = useGlobalSearchState(s => s.isLoading)
    const items = useGlobalSearchState(s => s.items)
    const currentDialog = useChatStore(store => store.dialogData);
    const selectDialog = useChatStore(s => s.selectDialogFromGlobalSearch)

    function isItemSelected(item: GlobalSearchItem) : boolean
    {
        if(item.itemType === GlobalSearchItemType.Chat)
            return currentDialog?.currentChat?.id === item.id;

        if(item.itemType === GlobalSearchItemType.User)
            return currentDialog?.userId === item.id;

        return false;
    }

    return (
        <div className={`${styles.container} ${isActive ? "" : styles.containerHidden}`}>
            {isLoading && <div className={styles.loadingBlock}>Loading...</div>}
            {(!isLoading && informMessage) &&
                <div className={styles.informMessage}>
                    <div className={styles.informMessageTitle}>{informMessage.title}</div>
                    <div className={styles.informMessageText}>{informMessage.text}</div>
                </div>
            }
            <div className={styles.content}>
                {items.map(item => <GlobalSearchItemComponent isSelected={isItemSelected(item)} key={`${item.itemType}-${item.id}`} item={item} selectDialog={selectDialog}/>)}
            </div>

        </div>
    );
};

export default GlobalSearchOverlay;