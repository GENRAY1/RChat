using RChat.Domain.Abstractions;

namespace RChat.Domain.Chats;

public class Chat : Entity
{
    public required ChatType Type { get; set; }
    public required int CreatorId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; set; } 
    public ChatGroup? GroupChat { get; set; }
    
    public bool IsClosed => Type == ChatType.Private || GroupChat?.IsPrivate == true;
}