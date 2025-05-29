import {AxiosResponse} from "axios";
import {apiClient} from "../api-client.ts";
import {GlobalSearchRequest, GlobalSearchResponse} from "./global-search-contracts.ts";

import {GlobalSearchItemType} from "../../models/global-search/GlobalSearchItemType.ts";
import {ChatSearchItem, UserSearchItem} from "../../models/global-search/GlobalSearchItem.ts";

export class GlobalSearchService {
    private static readonly path: string = 'globalSearch';

    public static async Search(request: GlobalSearchRequest): Promise<AxiosResponse<GlobalSearchResponse>> {
        const response =
            await  apiClient.get<GlobalSearchResponse>(`${this.path}`, {params: request});

        response.data = {
            chats: response.data.chats.map((chat): ChatSearchItem => ({
                ...chat,
                itemType: GlobalSearchItemType.Chat,
            })),
            users: response.data.users.map((user): UserSearchItem => ({
                ...user,
                itemType: GlobalSearchItemType.User
            })),
        };

        return response;
    }
}