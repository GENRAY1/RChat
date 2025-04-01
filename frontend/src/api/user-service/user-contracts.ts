import {UserChat} from "../../models/chat/UserChat.ts";

export interface GetMeChatsResponse{
    chats: Array<UserChat>
}