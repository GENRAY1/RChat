export interface CreateMessageRequest{
    chatId: number;
    text: string;
    replyToMessageId?: number;
}