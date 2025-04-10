import {Message} from "../message/Message.ts";
import {Chat} from "./Chat.ts";

export interface UserChat extends Chat{
    displayName: string;
    latestMessage?: Message
}