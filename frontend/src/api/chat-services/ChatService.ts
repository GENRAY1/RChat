import {AxiosResponse} from "axios";
import {apiClient} from "../api-client.ts";
import {
    CreateChatRequest,
    CreateChatResponse,
    GetChatMessagesRequest,
    GetChatMessagesResponse
} from "./chat-contracts.ts";

export class ChatService {

    private static readonly path: string = 'chats';

    public static async GetChatMessages(request: GetChatMessagesRequest): Promise<AxiosResponse<GetChatMessagesResponse>> {
        return apiClient.get<GetChatMessagesResponse>(`${this.path}/${request.chatId}/messages`, {params: request});
    }

    public static async GetChatMembers(request: GetChatMessagesRequest): Promise<AxiosResponse<GetChatMessagesResponse>> {
        return apiClient.get<GetChatMessagesResponse>(`${this.path}/${request.chatId}/members`, {params: request});
    }

    public static async CreateChat(request: CreateChatRequest): Promise<AxiosResponse<CreateChatResponse>> {
        return apiClient.post<CreateChatResponse>(this.path, request);
    }
}