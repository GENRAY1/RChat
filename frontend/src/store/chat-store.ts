import { create } from 'zustand'
import activeChatSlice, {ActiveChatSlice} from "./slices/active-chat-slice.ts";
import userChatsSlice, {UserChatsSlice} from "./slices/user-chats-slice";
import chatHubSlice, {ChatHubSlice} from "./slices/chat-hub-slice.ts";


export type ChatStore = ActiveChatSlice & UserChatsSlice & ChatHubSlice;

const useChatStore = create<ChatStore>((set, get, store) => ({
    ...activeChatSlice(set, get, store),
    ...userChatsSlice(set, get, store),
    ...chatHubSlice(set, get, store),
}));


export default useChatStore;