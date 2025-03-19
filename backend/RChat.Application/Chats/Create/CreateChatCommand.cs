using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Application.Chats.Create;

public class CreateChatCommand : ICommand<int>
{
    public required ChatType Type { get; set; }
    public required int CreatorId { get; init; }
    public ChatGroupDto? GroupDetails { get; set; }
    public int? RecipientId { get; init; } 
}