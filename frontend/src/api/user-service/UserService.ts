import axios, {AxiosResponse} from "axios";
import {AuthUser} from "../../models/user/AuthUser.ts";

export class UserService {

    private static url : string = 'http://localhost:5213/api/users';

    public static async GetMe() : Promise<AuthUser>{
        const response: AxiosResponse<AuthUser> = await axios.get(`${this.url}/me`);

        return response.data;
    }
}