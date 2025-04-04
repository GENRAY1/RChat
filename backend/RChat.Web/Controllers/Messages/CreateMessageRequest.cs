namespace RChat.Web.Controllers.Messages;

public class CreateMessageRequest
{
    public required int ChatId { get; set; }
    public required string Text { get; set; }
    public int? ReplyToMessageId { get; set; }
}