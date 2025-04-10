import {UserChat} from "../../models/chat/UserChat.ts";

export interface GetCurrentUserChatsResponse {
    chats: Array<UserChat>
}

export interface CreateUserRequest{
    firstname: string;
    lastname?: string;
    username?: string;
    description?: string;
    dateOfBirth?: Date;
}