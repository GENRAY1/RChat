using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Web.Controllers.Chats.GetById;

public class GetChatByIdResponse
{
    public int Id { get; init; }
    public ChatType Type { get; init; }
    public required int CreatorId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public ChatGroupDto? GroupChat { get; init; }
    
    public int MemberCount { get; init; }
}