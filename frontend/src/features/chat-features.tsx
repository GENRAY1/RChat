import {UserChat} from "../models/chat/UserChat.ts";
import {ChatType} from "../models/chat/ChatType.ts";

export function getDisplayName(chat: UserChat, currentUserId: number) : string | undefined{
    if(chat.type === ChatType.Group)
        return chat.groupChat?.name

    if(chat.type === ChatType.Private){
        const interlocutor =
            chat.privateChatMembers?.find(x => x.userId !== currentUserId);

        if (interlocutor)
            return `${interlocutor.firstname} ${interlocutor.lastname || ''}`.trim();
    }

    return undefined;
}
export function getInfo(chat: UserChat) : string | undefined  {
    if(chat.type === ChatType.Group)
        return `${chat.memberCount} members`;

    if(chat.type === ChatType.Private)
        return `Private chat`;
}