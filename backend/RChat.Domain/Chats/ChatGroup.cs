using RChat.Domain.Abstractions;

namespace RChat.Domain.Chats;

public class ChatGroup
{
    public const int MaxNameLength = 128;
    public const int MaxDescriptionLength = 255;
    
    public int ChatId { get; init; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required bool IsPrivate { get; set; }
}