namespace RChat.Web.Controllers.Messages.Update;

public class UpdateMessageResponse
{
    public required int MessageId { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required string Text { get; init; }
}