using RChat.Domain.Chats;

namespace RChat.Application.Chats.Dtos;

public class ChatDto
{
    public int Id { get; set; }
    public ChatType Type { get; init; }
    public required int CreatorId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public int MemberCount { get; init; }
    public ChatGroupDto? GroupChat { get; init; }
}