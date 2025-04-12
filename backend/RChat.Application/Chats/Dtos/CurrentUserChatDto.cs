using RChat.Application.Messages.Dtos;
using RChat.Domain.Chats;

namespace RChat.Application.Chats.Dtos;

public class CurrentUserChatDto
{
    public int Id { get; set; }
    
    public required string DisplayName { get; init; }
    
    public ChatType Type { get; init; }
    
    public required int CreatorId { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public int MemberCount { get; init; }
    
    public ChatGroupDto? GroupChat { get; init; }
    
    public MessageDto? LatestMessage { get; init; }
}