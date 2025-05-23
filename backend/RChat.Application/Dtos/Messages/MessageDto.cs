namespace RChat.Application.Dtos.Messages;

public class MessageDto
{
    public required int Id { get; init; }
    
    public required string Text { get; init; }
    
    public required int ChatId { get; init; }
    
    public int? ReplyToMessageId { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; }
    
    public DateTime? DeletedAt { get; init; }
    
    public required MessageSenderDto Sender { get; init; }
}