import {ChatType} from "../chat/ChatType.ts";
import {GlobalSearchType} from "./GlobalSearchType.ts";
import {UserSearchItem} from "./UserSearchItem.ts";

export type ChatSearchItem = {
    __type: GlobalSearchType.Chat;
    id: number;
    name: string;
    type: ChatType;
    memberCount: number;
}

export const isChatSearchItem = (item: ChatSearchItem | UserSearchItem): item is ChatSearchItem => {
    return item.__type === GlobalSearchType.Chat;
};