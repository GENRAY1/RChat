import {ChatSearchItem, UserSearchItem} from "../../models/global-search/GlobalSearchItem.ts";


export interface GlobalSearchResponse{
    chats: Array<ChatSearchItem>,
    users: Array<UserSearchItem>
}

export interface GlobalSearchRequest{
    query: string,
    take: number,
}
