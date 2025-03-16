namespace RChat.Application.Chats.Dtos;

public class ChatGroupDto
{
    public required string Name { get; init; }
    
    public string? Description { get; init; }
    
    public required bool IsPrivate { get; init; }
}