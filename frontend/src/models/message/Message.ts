import {MessageSender} from "./MessageSender.ts";

export interface Message{
    id: number,
    text: string,
    chatId: number,
    sender: MessageSender,
    replyToMessageId?: number,
    createdAt: Date,
    updatedAt?: Date,
    deletedAt?: Date,
}