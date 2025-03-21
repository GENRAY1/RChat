namespace RChat.Web.Hubs.Chats.SendMessage;

public class SendMessageRequest
{
    public required int ChatId { get; set; }
    public required string Text { get; set; }
    public int? ReplyToMessageId { get; set; }
}