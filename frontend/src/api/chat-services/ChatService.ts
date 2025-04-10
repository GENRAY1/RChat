import {AxiosResponse} from "axios";
import {apiClient} from "../api-client.ts";
import {GetChatMessagesRequest, GetChatMessagesResponse} from "./chat-contracts.ts";

export class ChatService {

    private static readonly path: string = 'chats';

    public static async GetChatMessages(request: GetChatMessagesRequest): Promise<AxiosResponse<GetChatMessagesResponse>> {
        const params = {
            skip: request.skip,
            take: request.take,
        }
        return apiClient.get<GetChatMessagesResponse>(`${this.path}/${request.chatId}/messages`, {params});
    }
}