import {Message} from "../../models/message/Message.ts"
import {ChatStore} from "./chat-store.ts";
import {StateCreator} from "zustand/vanilla";
import {GetChatMessagesResponse} from "../../api/chat-services/chat-contracts.ts";
import {ChatService} from "../../api/chat-services/ChatService.ts";
import {AxiosResponse} from "axios";
import {UserChat} from "../../models/chat/UserChat.ts";
import {ChatType} from "../../models/chat/ChatType.ts";
import {GlobalSearchItem} from "../../models/global-search/GlobalSearchItem.ts";
import {GlobalSearchItemType} from "../../models/global-search/GlobalSearchItemType.ts";
import {UserService} from "../../api/user-service/UserService.ts";
import {MembersService} from "../../api/members-service/members-service.ts";

export const TAKE_MESSAGES: number = 50;


export interface DialogAreaData{
    currentChat?: DialogChat;
    displayName: string;
    needCreatePrivateChat: boolean;
    userId?: number;
}

export interface DialogChat{
    id: number;
    type: ChatType;
    hasSubscription: boolean;
    memberCount: number;
}

export interface DialogAreaSlice {
    dialogData?: DialogAreaData;
    messages: Array<Message>;
    isLoadingMessages: boolean;
    skipMessages: number;
    hasMoreMessages: boolean;
    fetchMessages: (chatId: number, skip: number) => Promise<void>;
    selectDialogFromUserChats: (userChat: UserChat, displayName: string) => void;
    selectDialogFromGlobalSearch: (item: GlobalSearchItem, displayName: string) => Promise<void>;
    appendMessage: (message: Message) => void;
    lastAppendMessageId?: number;
    subscribe: (chatId: number) => Promise<void>;
}

const defaultState = {
    messages: [],
    dialogData: undefined,
    isLoadingMessages: false,
    hasMoreMessages: true,
    skipMessages: 0,
    dialogErrorMessage: undefined,
    lastAppendMessageId: undefined,
}

const dialogAreaSlice: StateCreator<ChatStore, [], [], DialogAreaSlice> = (set, get) : DialogAreaSlice =>({
    ...defaultState,
    selectDialogFromUserChats: (userChat: UserChat, displayName: string) => {
        const dialogChat: DialogChat = {
            id: userChat.id,
            type: userChat.type,
            hasSubscription: true,
            memberCount: userChat.memberCount
        };

        set({
            ...defaultState,
            dialogData : {needCreatePrivateChat: false, displayName, currentChat: dialogChat},
        });
    },
    selectDialogFromGlobalSearch: async (item:GlobalSearchItem, displayName: string) => {
        const params = item.itemType === GlobalSearchItemType.User
            ? { skip: 0, take: 1, secondUserId: item.id, type:ChatType.Private}
            : { skip: 0, take: 1, chatIds: [item.id] };

        const userChatResponse = await UserService.GetUserChats(params);
        const userChat: UserChat | undefined = userChatResponse.data.chats?.[0];

        let dialogChat: DialogChat | undefined;

        if(item.itemType === GlobalSearchItemType.Chat){
            dialogChat = {id: item.id, type:item.type, hasSubscription: userChat !== undefined,  memberCount: item.memberCount}
        }

        if(userChat && item.itemType === GlobalSearchItemType.User)
            dialogChat = {id: userChat.id, type:userChat.type, hasSubscription: true,  memberCount: userChat.memberCount}


        set({
            ...defaultState,
            dialogData: {
                displayName,
                userId: item.itemType === GlobalSearchItemType.User ? item.id : undefined,
                needCreatePrivateChat: item.itemType === GlobalSearchItemType.User && userChat === undefined,
                currentChat: dialogChat
            }
        });
    },
    appendMessage: (message:Message) => {
        set((state) =>
        ({
            messages: [...state.messages, message],
            lastAppendMessageId: message.id
        }))

    },
    fetchMessages:async (chatId: number, skip: number):Promise<void> => {
        set({isLoadingMessages: true})

        try{
            const response:AxiosResponse<GetChatMessagesResponse> = await ChatService.GetChatMessages(
                {
                    chatId:chatId,
                    take: TAKE_MESSAGES,
                    skip: skip
                })

            const messages:Array<Message> = response.data.messages.reverse();

            const hasMore : boolean = messages.length === TAKE_MESSAGES

            set((state) => ({
                messages: [...messages, ...state.messages],
                hasMoreMessages: hasMore,
                skipMessages: state.skipMessages + messages.length,
            }))
        }
        finally{
            set({isLoadingMessages: false})
        }
    },

    subscribe:async (chatId: number) =>{
        const {joinChat} = get()

        await MembersService.CreateMember({chatId: chatId});
        await joinChat(chatId);

        set(s => {
            if(s.dialogData?.currentChat?.id === chatId)
                return {
                    dialogData: {
                        ...s.dialogData,
                        currentChat: { ...s.dialogData.currentChat, hasSubscription: true, memberCount:s.dialogData.currentChat.memberCount + 1}
                    }
                }
            return{}
        })
    }
})

export default dialogAreaSlice;