import {HubConnection, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";
import {ChatStore} from "../chat-store.ts";
import {StateCreator} from "zustand/vanilla";
import {Message} from "../../models/message/Message.ts";

export interface ChatHubSlice {
    connection?: HubConnection;
    isConnected: boolean;
    startConnection: (accessToken:string) => Promise<void>;
    stopConnection: () => Promise<void>;
}

const CHAT_HUB_URL:string = "http://localhost:5213/hubs/chat"

enum ChatHubEvents {
    ReceiveMessage = "ReceiveMessage",
    MessageUpdated = "MessageUpdated",
    MessageDeleted = "MessageDeleted",
}

const chatHubSlice: StateCreator<ChatStore, [], [], ChatHubSlice> = (set,get) => ({
    connection: undefined,
    isConnected: false,
    startConnection:  async (accessToken: string) =>{

        const {connection} = get();

        if(connection){
            console.log("CHAT HUB is already connected");
            return;
        }

        try {
            const connection = new HubConnectionBuilder()
                .withUrl(CHAT_HUB_URL, {accessTokenFactory: ()=> accessToken})
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            connection.on(ChatHubEvents.ReceiveMessage, (message: Message) => {
                const {activeChat, appendMessage, updateUserChatLatestMessage} = get();

                //For fix dates
                const newMessage : Message = {
                    ...message,
                    createdAt : new Date(message.createdAt),
                    deletedAt: message.deletedAt
                        ? new Date(message.deletedAt)
                        : undefined,
                    updatedAt: message.updatedAt
                        ? new Date(message.createdAt)
                        : undefined
                }

                if(activeChat){
                    appendMessage(newMessage)
                }

                updateUserChatLatestMessage(message.chatId, newMessage)
            });

            connection.onclose(() => {
                set({ isConnected: false });
            });

            await connection.start();
            set({connection, isConnected:true});
            console.log("CHAT HUB connected");
        }
        catch (err){
            console.log("CHAT HUB connection error:", err);
        }

    },
    stopConnection: async () =>{
        const { connection } = get();
        if (connection) {
            await connection.stop();
            set({ connection: undefined, isConnected: false });
            console.log("CHAT HUB disconnected");
        }else {
            console.log("CHAT HUB is already disconnected");
        }
    }
});

export default chatHubSlice;