import {UserChat} from "../../models/chat/UserChat.ts";
import {UserService} from "../../api/user-service/UserService.ts";
import {ApiErrorData, getApiErrorOrDefault} from "../../api/api-error-data.ts";

import {StateCreator} from "zustand/vanilla";
import {ChatStore} from "../chat-store.ts";
import {Message} from "../../models/message/Message.ts";

export interface UserChatsSlice{
    userChats: UserChat[],
    fetchChatsErrorMsg?: string,
    updateChatLatestMessage: (chatId: number, message: Message) => void
    fetchChats:() => Promise<void>,

}
const userChatsSlice: StateCreator<ChatStore, [], [], UserChatsSlice> = (set) => ({
    userChats: [],
    fetchChatsErrorMsg: undefined,
    updateChatLatestMessage: (chatId, message) : void =>{
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
    fetchChats: async () => {
        try {
            const response = await UserService.GetCurrentUserChats();
            set({ userChats: [...response.data.chats] });
        } catch (err) {
            const apiError: ApiErrorData = getApiErrorOrDefault(err);
            set({ fetchChatsErrorMsg: apiError.Message });
        }
    }
});


export default userChatsSlice;