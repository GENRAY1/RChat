import { create } from 'zustand'
import userChatsSlice, {UserChatsSlice} from "./user-chats-slice.ts";
import chatHubSlice, {ChatHubSlice} from "./chat-hub-slice.ts";
import dialogAreaSlice, {DialogAreaSlice} from "./dialog-area-slice.ts";


export type ChatStore = //ActiveChatSlice
    & UserChatsSlice & ChatHubSlice & DialogAreaSlice;

const useChatStore = create<ChatStore>((set, get, store) => ({
    ...userChatsSlice(set, get, store),
    ...chatHubSlice(set, get, store),
    ...dialogAreaSlice(set, get, store),
}));


export default useChatStore;