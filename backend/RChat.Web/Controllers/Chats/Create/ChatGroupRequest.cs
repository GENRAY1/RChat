namespace RChat.Web.Controllers.Chats.Create;

public class ChatGroupRequest
{
    public required string Name { get; init; }
    
    public string? Description { get; init; }
    
    public required bool IsPrivate { get; init; }
}