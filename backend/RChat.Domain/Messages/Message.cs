using RChat.Domain.Abstractions;

namespace RChat.Domain.Messages;

public class Message : Entity
{
    public const int MaxTextLength = 4096;
    
    public required string Text { get; set; }
    
    public required int ChatId { get; set; }
    
    public required int SenderId { get; set; }
    
    public int? ReplyToMessageId { get; set; }
    
    public required DateTime CreatedAt { get; init; }
    
    public DateTime? DeletedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
}