import {ChatSearchItem} from "../../models/global-search/ChatSearchItem.ts";
import {UserSearchItem} from "../../models/global-search/UserSearchItem.ts";

export interface GlobalSearchResponse{
    chats: Array<ChatSearchItem>,
    users: Array<UserSearchItem>
}

export interface GlobalSearchRequest{
    query: string,
    take: number,
}
