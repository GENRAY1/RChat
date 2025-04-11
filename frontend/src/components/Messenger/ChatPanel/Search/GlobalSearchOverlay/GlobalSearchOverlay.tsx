import styles from './GlobalSearchOverlay.module.css'
import useGlobalSearchState from "../../../../../store/global-search-store/global-search-store.ts";
import GlobalSearchItem from "../GlobalSearchItem/GlobalSearchItem.tsx";

const GlobalSearchOverlay = () => {
    const isActive = useGlobalSearchState(s => s.isOverlayActive)
    const informMessage = useGlobalSearchState(s => s.message)
    const isLoading = useGlobalSearchState(s => s.isLoading)
    const items = useGlobalSearchState(s => s.items)

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
                {items.map(item => <GlobalSearchItem item={item}></GlobalSearchItem>)}
            </div>

        </div>
    );
};

export default GlobalSearchOverlay;