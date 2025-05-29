import {UserChat} from "../../models/chat/UserChat.ts";
import {UserService} from "../../api/user-service/UserService.ts";
import {ApiErrorData, getApiErrorOrDefault} from "../../api/api-error-data.ts";

import {StateCreator} from "zustand/vanilla";
import {ChatStore} from "./chat-store.ts";
import {Message} from "../../models/message/Message.ts";
import {ChatType} from "../../models/chat/ChatType.ts";
import {CreateChatRequest} from "../../api/chat-services/chat-contracts.ts";
import {ChatService} from "../../api/chat-services/ChatService.ts";

export interface UserChatsSlice{
    userChats: UserChat[]
    fetchUserChatsErrorMsg?: string
    updateUserChatLatestMessage: (chatId: number, message: Message) => void
    fetchUserChats:() => Promise<void>
    userChatTypeFilter?: ChatType
    setUserChatTypeFilter: (type?: ChatType) => void
    createChat: (request:CreateChatRequest) => Promise<UserChat>
}

const TAKE_CHATS: number = 200;

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
            const response = await UserService.GetUserChats({
                take: TAKE_CHATS,
                skip: 0
            });
            set({ userChats: [...response.data.chats] });
        } catch (err) {
            const apiError: ApiErrorData = getApiErrorOrDefault(err);
            set({ fetchUserChatsErrorMsg: apiError.Message });
        }
    },
    userChatTypeFilter: undefined,
    setUserChatTypeFilter:(type?:ChatType) => set({userChatTypeFilter: type}),
    createChat: async (request:CreateChatRequest):Promise<UserChat>=>{
        const { userChatTypeFilter, joinChat } = get();
        const createChatResponse = await ChatService.CreateChat(request)

        const createdChat:UserChat = {
            id: createChatResponse.data.id,
            type: createChatResponse.data.type,
            creatorId: createChatResponse.data.creatorId,
            createdAt:createChatResponse.data.createdAt,
            deletedAt: createChatResponse.data.deletedAt,
            memberCount: createChatResponse.data.memberCount,
            groupChat: createChatResponse.data.groupChat,
            privateChatMembers: createChatResponse.data.privateChatMembers,
        }

        if(!userChatTypeFilter || request.type === userChatTypeFilter){
            set(state => ({userChats: [createdChat, ...state.userChats]}))
        }

        if(request.recipientId && request.type === ChatType.Private){
            set(state => {
                if(state.dialogData && state.dialogData.userId === request.recipientId){
                    return  {
                        dialogData: {
                            ...state.dialogData,
                            needCreatePrivateChat: false,
                            currentChat: {id: createdChat.id, type: createdChat.type, hasSubscription: true, memberCount:createdChat.memberCount}
                        },
                    }
                }

                return {}
            })
        }

        await joinChat(createdChat.id)

        return createdChat;
    }
});

export default userChatsSlice;