import {
    LoginRequest,
    LoginResponse,
    RegisterRequest,
    RegisterResponse
} from "./account-contracts.ts";
import {AxiosResponse} from "axios";
import {CurrentAccount} from "../../models/account/CurrentAccount.ts";
import {apiClient} from "../api-client.ts";

export class AccountService {

    private static path : string = 'account';

    public static async login(request: LoginRequest) : Promise<AxiosResponse<LoginResponse>>{
        return apiClient.post<LoginResponse>(`${this.path}/login`, request);
    }

    public static async register(request: RegisterRequest) : Promise<AxiosResponse<RegisterResponse>>{
        return await apiClient.post<RegisterResponse>(`${this.path}/register`, request);

    }
    public static async getCurrent() : Promise<AxiosResponse<CurrentAccount>>{
        return await apiClient.get<CurrentAccount>(`${this.path}/me`);
    }
}