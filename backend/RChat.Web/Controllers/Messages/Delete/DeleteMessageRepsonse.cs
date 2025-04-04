namespace RChat.Web.Controllers.Messages.Delete;

public class DeleteMessageResponse
{
    public required int MessageId { get; init; }
    public required DateTime DeletedAt { get; init; }
}