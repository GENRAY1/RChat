import { Message } from "../../models/message/Message.ts"
import {ChatStore} from "./chat-store.ts";
import {StateCreator} from "zustand/vanilla";
import {UserChat} from "../../models/chat/UserChat.ts";
import {GetChatMessagesResponse} from "../../api/chat-services/chat-contracts.ts";
import {ChatService} from "../../api/chat-services/ChatService.ts";
import {AxiosResponse} from "axios";
import {ApiErrorData, getApiErrorOrDefault} from "../../api/api-error-data.ts";

export const TAKE_MESSAGES: number = 50;

export interface ActiveChatSlice{
    activeChat?: UserChat,
    messages: Array<Message>,
    isLoadingMessages: boolean,
    skipMessages: number,
    hasMoreMessages: boolean,
    fetchMessagesErrorMsg?: string,
    fetchMessages: () => Promise<void>,
    activateChat: (activeChat: UserChat) => void,
    appendMessage: (message: Message) => void,
    lastAppendMessageId?: number,
}

const defaultState = {
    activeChat: undefined,
    messages: [],
    isLoadingMessages: false,
    hasMoreMessages: true,
    skipMessages: 0,
    fetchMessagesErrorMsg: undefined,
    lastAppendMessageId: undefined,
}

const activeChatSlice: StateCreator<ChatStore, [], [], ActiveChatSlice> = (set, get) : ActiveChatSlice =>({
    ...defaultState,
    activateChat: (chat: UserChat) => {

        const {activeChat} = get();

        if(!activeChat || activeChat.id !== chat.id){
            set({...defaultState, activeChat: chat});
        }
    },
    appendMessage: (message:Message) => {
        set((state) =>
        ({
            messages: [...state.messages, message],
            lastAppendMessageId: message.id
        }))

    },
    fetchMessages:async ():Promise<void> => {
        const { isLoadingMessages, hasMoreMessages, skipMessages, activeChat} = get();

        if(isLoadingMessages || !hasMoreMessages || !activeChat) return

        set({isLoadingMessages: true, fetchMessagesErrorMsg: undefined})

        try{
            const response:AxiosResponse<GetChatMessagesResponse> = await ChatService.GetChatMessages(
                {
                    chatId:activeChat.id,
                    take: TAKE_MESSAGES,
                    skip: skipMessages
                })

            const messages:Array<Message> = response.data.messages.reverse();

            const hasMore : boolean = messages.length === TAKE_MESSAGES

            set((state) => ({
                messages: [...messages, ...state.messages,],
                hasMoreMessages: hasMore,
                skipMessages: state.skipMessages + messages.length,
            }))
        }catch (error) {
            const apiError:ApiErrorData = getApiErrorOrDefault(error);

            set({fetchMessagesErrorMsg: apiError.Message})
        }
        finally{
            set({isLoadingMessages: false})
        }
    }
})

export default activeChatSlice;