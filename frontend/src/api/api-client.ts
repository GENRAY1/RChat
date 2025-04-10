import axios, {AxiosInstance} from "axios";
import {addAxiosDateTransformer} from "axios-date-transformer";


export const apiClient: AxiosInstance = axios.create({
    baseURL:"http://localhost:5213/api/"
});

addAxiosDateTransformer(apiClient)