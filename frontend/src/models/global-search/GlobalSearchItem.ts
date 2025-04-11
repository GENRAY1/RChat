import {GlobalSearchType} from "./GlobalSearchType.ts";

export interface GlobalSearchItem{
    id: number;
    displayName: string;
    type: GlobalSearchType;
    username?: string;
    membersCount?: number;
}