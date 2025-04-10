import {Message} from "../../models/message/Message.ts";
import {Pagination} from "../../models/common/Pagination.ts";

export interface GetChatMessagesResponse{
    messages: Array<Message>
}

export interface GetChatMessagesRequest extends Pagination{
    chatId: number;
}