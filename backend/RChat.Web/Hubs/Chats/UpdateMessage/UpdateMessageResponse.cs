namespace RChat.Web.Hubs.Chats.UpdateMessage;

public class UpdateMessageResponse
{
    public required int MessageId { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required string Text { get; init; }
}