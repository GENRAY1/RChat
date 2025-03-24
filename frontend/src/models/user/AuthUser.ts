import {Role} from "./Role.ts";

export interface AuthUser {
    id: number,
    login: string,
    username: string,
    description?: string,
    dateOfBirth?: Date,
    createdAt: Date,
    role: Role,
}