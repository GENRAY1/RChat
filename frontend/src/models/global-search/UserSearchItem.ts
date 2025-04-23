import {GlobalSearchType} from "./GlobalSearchType.ts";
import {ChatSearchItem} from "./ChatSearchItem.ts";

export type UserSearchItem = {
    __type: GlobalSearchType.User;
    id: number;
    firstname: string;
    lastname?: string;
    username?: string;
}

export const isUserSearchItem = (item: ChatSearchItem | UserSearchItem): item is UserSearchItem => {
    return item.__type === GlobalSearchType.User;
};