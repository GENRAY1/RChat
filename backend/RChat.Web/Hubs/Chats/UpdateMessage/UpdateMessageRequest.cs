namespace RChat.Web.Hubs.Chats.UpdateMessage;

public class UpdateMessageRequest
{
    public required int MessageId { get; init; }
    public required string Text { get; init; }
}