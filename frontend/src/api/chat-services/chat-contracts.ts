import {Message} from "../../models/message/Message.ts";
import {Pagination} from "../../models/common/Pagination.ts";
import {ChatType} from "../../models/chat/ChatType.ts";
import {ChatGroup} from "../../models/chat/ChatGroup.ts";
import {Chat} from "../../models/chat/Chat.ts";
import {PrivateChatMember} from "../../models/chat/PrivateChatMembers.ts";

export interface GetChatMessagesResponse{
    messages: Array<Message>;
}

export interface GetChatMessagesRequest extends Pagination{
    chatId: number;
}

export interface GetChatMembersRequest extends Pagination{
    chatId: number;
}

export interface CreateChatRequest{
    type:ChatType;
    groupChat?:ChatGroup;
    recipientId?: number;
}

export interface CreateChatResponse extends Chat {
    privateChatMembers:PrivateChatMember[];
}