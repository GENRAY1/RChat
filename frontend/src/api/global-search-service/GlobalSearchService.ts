import {AxiosResponse} from "axios";
import {apiClient} from "../api-client.ts";
import {GlobalSearchRequest, GlobalSearchResponse} from "./global-search-contracts.ts";
import {UserSearchItem} from "../../models/global-search/UserSearchItem.ts";
import {ChatSearchItem} from "../../models/global-search/ChatSearchItem.ts";
import {GlobalSearchType} from "../../models/global-search/GlobalSearchType.ts";

export class GlobalSearchService {
    private static readonly path: string = 'globalSearch';

    public static async Search(request: GlobalSearchRequest): Promise<AxiosResponse<GlobalSearchResponse>> {
        const response =
            await  apiClient.get<GlobalSearchResponse>(`${this.path}`, {params: request});

        response.data = {
            chats: response.data.chats.map((chat): ChatSearchItem => ({
                ...chat,
                __type: GlobalSearchType.Chat,
            })),
            users: response.data.users.map((user): UserSearchItem => ({
                ...user,
                __type: GlobalSearchType.User
            })),
        };

        return response;
    }
}