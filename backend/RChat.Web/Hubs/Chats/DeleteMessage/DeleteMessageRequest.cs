namespace RChat.Web.Hubs.Chats.DeleteMessage;

public class DeleteMessageRequest
{
    public required int MessageId { get; init; }
}