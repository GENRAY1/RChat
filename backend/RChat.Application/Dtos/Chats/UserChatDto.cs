using RChat.Application.Dtos.Messages;
using RChat.Domain.Chats;

namespace RChat.Application.Dtos.Chats;

public class UserChatDto
{
    public int Id { get; set; }
    public ChatType Type { get; init; }
    public required int CreatorId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public int MemberCount { get; init; }
    public ChatGroupDto? GroupChat { get; init; }
    public PrivateChatMembersDto? PrivateChatMembers { get; init; } 
    public MessageDto? LatestMessage { get; init; }
}