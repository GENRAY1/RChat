namespace RChat.Application.Services.Search.Message;

public class MessageDocument
{
    public required string Text { get; init; }
    public required int ChatId { get; init; }
    public DateTime? DeletedAt { get; init; }
    public required int SenderId { get; init; }
}