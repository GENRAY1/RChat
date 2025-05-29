import {Message} from "../message/Message.ts";
import {Chat} from "./Chat.ts";
import {PrivateChatMember} from "./PrivateChatMembers.ts";

export interface UserChat extends Chat{
    privateChatMembers?: PrivateChatMember[];
    latestMessage?: Message
}