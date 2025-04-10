import {CreateMessageRequest} from "./messages-contracts.ts";
import {AxiosResponse} from "axios";
import {Message} from "../../models/message/Message.ts";
import {apiClient} from "../api-client.ts";

export class MessageService {

    public static readonly path: string = 'messages';

    public static async CreateMessage(request: CreateMessageRequest) : Promise<AxiosResponse<Message>> {
        return apiClient.post<Message>(`${this.path}`, request);
    }
}