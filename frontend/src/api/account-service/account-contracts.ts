export interface LoginResponse {
    accessToken: string
}

export interface LoginRequest {
    login: string,
    password: string,
}

export interface RegisterResponse {
    id: string
}

export interface RegisterRequest {
    login: string,
    password: string,
}
