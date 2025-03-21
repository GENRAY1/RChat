namespace RChat.Web.Hubs.Chats.DeleteMessage;

public class DeleteMessageResponse
{
    public required int MessageId { get; init; }
    public required DateTime DeletedAt { get; init; }
}