export interface User {
    id: number,
    accountId: string,
    firstName: string,
    lastName?: string,
    username?: string,
    description?: string,
    dateOfBirth?: Date,
    createdAt: Date,
    updatedAt?: Date
}