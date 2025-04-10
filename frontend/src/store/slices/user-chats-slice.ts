import {UserChat} from "../../models/chat/UserChat.ts";
import {UserService} from "../../api/user-service/UserService.ts";
import {ApiErrorData, getApiErrorOrDefault} from "../../api/api-error-data.ts";

import {StateCreator} from "zustand/vanilla";
import {ChatStore} from "../chat-store.ts";
import {Message} from "../../models/message/Message.ts";
import {ChatType} from "../../models/chat/ChatType.ts";

export interface UserChatsSlice{
    userChats: UserChat[]
    fetchUserChatsErrorMsg?: string
    updateUserChatLatestMessage: (chatId: number, message: Message) => void
    fetchUserChats:() => Promise<void>
    userChatTypeFilter?: ChatType
    setUserChatTypeFilter: (type?: ChatType) => void
}



const userChatsSlice: StateCreator<ChatStore, [], [], UserChatsSlice> = (set, get) => ({
    userChats: [],
    fetchUserChatsErrorMsg: undefined,
    updateUserChatLatestMessage: (chatId, message) : void =>{
        set(state => {
            const chatExists = state.userChats.some(chat => chat.id === chatId);
            if (!chatExists) return {};

            return {
                userChats: state.userChats.map(chat =>
                    chat.id === chatId
                        ? { ...chat, latestMessage: message }
                        : chat
                )
            }
        });
    },
    fetchUserChats: async () => {
        try {
            const response = await UserService.GetCurrentUserChats();
            set({ userChats: [...response.data.chats] });
        } catch (err) {
            const apiError: ApiErrorData = getApiErrorOrDefault(err);
            set({ fetchUserChatsErrorMsg: apiError.Message });
        }
    },
    userChatTypeFilter: undefined,
    setUserChatTypeFilter:(type?:ChatType) => set({userChatTypeFilter: type})
});

export default userChatsSlice;