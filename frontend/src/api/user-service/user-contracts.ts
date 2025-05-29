import {UserChat} from "../../models/chat/UserChat.ts";
import {Pagination} from "../../models/common/Pagination.ts";
import {ChatType} from "../../models/chat/ChatType.ts";

export interface GetUserChatsResponse {
    chats: Array<UserChat>
}

export interface GetUserChatsRequest extends Pagination{
    secondUserId?: number;
    chatIds?: number[];
    type?: ChatType;
}

export interface CreateUserRequest{
    firstname: string;
    lastname?: string;
    username?: string;
    description?: string;
    dateOfBirth?: Date;
}