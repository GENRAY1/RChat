import axios, {AxiosInstance} from "axios";
import {addAxiosDateTransformer} from "axios-date-transformer";


export const apiClient: AxiosInstance = axios.create({
    baseURL:"http://localhost:5213/api/",
    paramsSerializer: (params) => {
        const searchParams = new URLSearchParams();

        for (const key in params) {
            const value = params[key];
            if (Array.isArray(value)) {
                value.forEach(val => searchParams.append(key, val));
            } else {
                searchParams.append(key, value);
            }
        }

        return searchParams.toString();
    }
});

addAxiosDateTransformer(apiClient)
