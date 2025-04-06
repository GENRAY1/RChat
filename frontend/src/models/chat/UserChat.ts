import {ChatType} from "./ChatType.ts";
import {ChatGroup} from "./ChatGroup.ts";
import {Message} from "../message/Message.ts";

export interface UserChat{
    id: number;
    displayName: string;
    type: ChatType;
    creatorId: number;
    createdAt: Date;
    groupChat: ChatGroup;
    latestMessage: Message
}