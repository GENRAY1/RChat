namespace RChat.Application.Messages.SoftDelete;

public class DeleteMessageDtoResponse
{
    public required int MessageId { get; init; }
    public required DateTime DeletedAt { get; init; }
    
    public required int ChatId { get; init; }
}