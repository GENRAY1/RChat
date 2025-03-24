import {LoginRequest, LoginResponse, RegisterRequest, RegisterResponse} from "./auth-contracts.ts";
import axios, {AxiosResponse} from "axios";

export class AuthService{

    private static url : string = 'http://localhost:5213/api/auth';

    public static async login(request: LoginRequest) : Promise<LoginResponse>{
        const response: AxiosResponse<LoginResponse> = await axios.post(`${this.url}/login`, request);

        return response.data;
    }

    public static async register(request: RegisterRequest) : Promise<RegisterResponse>{
        const response: AxiosResponse<RegisterResponse> = await axios.post(`${this.url}/register`, request);

        return response.data;
    }

}