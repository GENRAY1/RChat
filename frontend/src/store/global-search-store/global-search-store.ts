import {create} from "zustand";
import {GlobalSearchMessage} from "../../models/global-search/GlobalSearchMessage.ts";
import {ChatSearchItem} from "../../models/global-search/ChatSearchItem.ts";
import {UserSearchItem} from "../../models/global-search/UserSearchItem.ts";
import {debounce} from "lodash";
import {GlobalSearchService} from "../../api/global-search-service/GlobalSearchService.ts";

interface SearchState {
    query: string;
    items: Array<ChatSearchItem | UserSearchItem>;
    isOverlayActive: boolean;
    search: (query: string) => Promise<void>;
    openOverlay: ()=> void;
    closeOverlay: ()=> void;
    message?: GlobalSearchMessage;
    isLoading: boolean;
}

const DEFAULT_SEARCH_MESSAGE : GlobalSearchMessage = {
    title: "Information",
    text: "Enter text to search for chats, users"
}

const GLOBAL_SEARCH_LIMIT : number = 10;

const DEFAULT_STATE_VALUES = {
    query: '',
    items: [],
    isOverlayActive: false,
    isLoading: false,
    message: DEFAULT_SEARCH_MESSAGE
}

const useGlobalSearchState = create<SearchState>((set) => {
    const debouncedSearch = debounce(async (query:string) => {
        set({isLoading: true});

        try {
            const response = await GlobalSearchService.Search({query, take: GLOBAL_SEARCH_LIMIT});
            const items = [...response.data.chats, ...response.data.users];

            set({
                items,
                message: items.length === 0
                    ? {title: "No results", text: "Nothing found for your query"}
                    : undefined,
                isLoading: false
            });
        } catch {
            set({
                items: [],
                message: {title: "Error", text: "Search failed"},
                isLoading: false
            });
        }
    }, 600);

    return {
        ...DEFAULT_STATE_VALUES,
        search: async (query: string) => {
            if (!query.trim()) {
                debouncedSearch.cancel()
                set(
                    {
                        query: '',
                        isLoading: false,
                        items: [],
                        message: DEFAULT_SEARCH_MESSAGE
                    }
                )
            }else{
                set({query})
                await debouncedSearch(query)
            }
        },
        openOverlay: () => set({isOverlayActive: true}),
        closeOverlay: () => {
            set({...DEFAULT_STATE_VALUES});
            debouncedSearch.cancel();
        },
    }
});

export default useGlobalSearchState;