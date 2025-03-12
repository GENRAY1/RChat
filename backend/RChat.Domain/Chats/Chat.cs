using RChat.Domain.Abstractions;

namespace RChat.Domain.Chats;

public class Chat : Entity
{
    public required ChatType Type { get; set; }
    
    public required int CreatorId { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public ChatGroup? GroupChat { get; set; }
}