namespace RChat.Application.Extensions.Messages;

public class UpdateMessageDtoResponse
{
    public required int MessageId { get; init; }
    
    public required int ChatId { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required string Text { get; init; }
}