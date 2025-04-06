import axios, {AxiosInstance} from "axios";

export const apiClient: AxiosInstance = axios.create({
    baseURL:"http://localhost:5213/api/"
});