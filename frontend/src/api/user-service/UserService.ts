
import {CreateUserRequest, GetUserChatsRequest, GetUserChatsResponse} from "./user-contracts.ts";
import {User} from "../../models/user/User.ts";
import {apiClient} from "../api-client.ts";
import {AxiosResponse} from "axios";

export class UserService {

    private static path : string = 'users';

    public static async Create(request: CreateUserRequest) : Promise<AxiosResponse<User>>{
        return apiClient.post(`${this.path}`, request);
    }

    public static async GetUserChats(request: GetUserChatsRequest):Promise<AxiosResponse<GetUserChatsResponse>>{
        return apiClient.get<GetUserChatsResponse>(`${this.path}/me/chats`, {params: request});
    }
}