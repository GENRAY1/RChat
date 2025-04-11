import {create} from "zustand";
import {GlobalSearchMessage} from "../../models/global-search/GlobalSearchMessage.ts";
import {GlobalSearchItem} from "../../models/global-search/GlobalSearchItem.ts";

interface SearchState {
    query: string;
    items: Array<GlobalSearchItem>;
    isOverlayActive: boolean;
    setQuery: (query: string) => void;
    openOverlay: ()=> void;
    closeOverlay: ()=> void;
    message?: GlobalSearchMessage;
    isLoading: boolean;
}

const defaultStateValues = {
    query: '',
    items: [],
    isOverlayActive: false,
    isLoading: false,
    message: {
        title: "Information",
        text: "Enter text to search for chats, users"
    }
}

const useGlobalSearchState = create<SearchState>((set) => ({
    ...defaultStateValues,
    setQuery: (query: string) => set({query}),
    openOverlay: ()=>set({isOverlayActive: true}),
    closeOverlay: ()=> {
        set({...defaultStateValues});
    },
}));

export default useGlobalSearchState;