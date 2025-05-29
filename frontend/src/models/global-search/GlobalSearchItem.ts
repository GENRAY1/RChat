import {ChatType} from "../chat/ChatType.ts";
import {GlobalSearchItemType} from "./GlobalSearchItemType.ts";


export type ChatSearchItem = {
    itemType: GlobalSearchItemType.Chat;
    id: number;
    name: string;
    type: ChatType;
    memberCount: number;
}

export type UserSearchItem = {
    itemType: GlobalSearchItemType.User;
    id: number;
    firstname: string;
    lastname?: string;
    username?: string;
}

export type GlobalSearchItem = UserSearchItem | ChatSearchItem;