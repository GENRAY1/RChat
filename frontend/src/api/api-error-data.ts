import {useState} from "react";
import {AxiosResponse, isAxiosError} from "axios";

export interface ApiErrorData {
    Message: string;
    CodeMessage?: string;
}

export const DEFAULT_ERROR:ApiErrorData = {
    Message: "Something went wrong. Please try again later",
    CodeMessage: undefined
};

export const getApiErrorOrDefault = (error: unknown) : ApiErrorData => {
    if (isAxiosError(error) && error.response?.data) {
        return (error.response.data as ApiErrorData);
    }

    return DEFAULT_ERROR;
};

export function useApi<T>() {
    const [data, setData] = useState<T | null>(null);
    const [error, setError] = useState<ApiErrorData | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const execute = async (promise: Promise<AxiosResponse<T>>) => {
        setIsLoading(true);
        setError(null);

        try {
            const result = await promise;
            setData(result.data);
            return result;
        } catch (error) {
            const apiError = getApiErrorOrDefault(error);
            setError(apiError)
            throw error;
        } finally {
            setIsLoading(false);
        }
    };

    return {data, error, isLoading, execute};
}