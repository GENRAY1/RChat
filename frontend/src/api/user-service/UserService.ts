
import {CreateUserRequest, GetCurrentUserChatsResponse} from "./user-contracts.ts";
import {User} from "../../models/user/User.ts";
import {apiClient} from "../api-client.ts";
import {AxiosResponse} from "axios";

export class UserService {

    private static path : string = 'users';

    public static async Create(request: CreateUserRequest) : Promise<AxiosResponse<User>>{
        return apiClient.post(`${this.path}`, request);
    }

    public static async GetCurrentUserChats():Promise<AxiosResponse<GetCurrentUserChatsResponse>>{
        return apiClient.get<GetCurrentUserChatsResponse>(`${this.path}/me/chats`);
    }
}