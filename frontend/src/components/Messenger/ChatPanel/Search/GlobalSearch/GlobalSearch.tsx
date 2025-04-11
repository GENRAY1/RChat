import { FC} from 'react';
import styles from './GlobalSearch.module.css'
import useGlobalSearchState from "../../../../../store/global-search-store/global-search-store.ts";


const GlobalSearch : FC= () => {
    const isActive = useGlobalSearchState(s => s.isOverlayActive)
    const query = useGlobalSearchState(x => x.query)
    const setQuery = useGlobalSearchState(x => x.setQuery)
    const openSearchOverlay = useGlobalSearchState(x => x.openOverlay)
    const closeSearchOverlay = useGlobalSearchState(x => x.closeOverlay)

    return (
        <div className={styles.container}>
            <input
                type="text"
                placeholder="search"
                autoComplete="off"
                value= {query}
                onFocus={()=> openSearchOverlay()}
                onChange={(e) => setQuery(e.target.value)}
                className={styles.searchInput}
            />
            {isActive && <button onClick={() => closeSearchOverlay()} className={styles.closeButton}>✖</button>}
        </div>
    );
};

export default GlobalSearch;