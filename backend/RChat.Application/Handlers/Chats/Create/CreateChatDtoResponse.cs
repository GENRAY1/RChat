using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Handlers.Chats.Create;

public class CreateChatDtoResponse
{
    public int Id { get; set; }
    public ChatType Type { get; init; }
    public required int CreatorId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public int MemberCount { get; init; }
    public ChatGroupDto? GroupChat { get; init; }
    public List<PrivateChatMemberDto>? PrivateChatMembers { get; init; } 
}