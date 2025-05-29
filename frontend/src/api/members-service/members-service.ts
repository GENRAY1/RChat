import {AxiosResponse} from "axios";
import {apiClient} from "../api-client.ts";
import {CreateMemberRequest, CreateMemberResponse} from "./members-service-contracts.ts";

export class MembersService{
    public static path:string = "/members";
    public static async CreateMember(request: CreateMemberRequest): Promise<AxiosResponse<CreateMemberResponse>> {
        return apiClient.post<CreateMemberResponse>(this.path, request);
    }
}