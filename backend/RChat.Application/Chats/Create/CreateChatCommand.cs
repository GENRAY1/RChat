using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Application.Chats.Create;

public class CreateChatCommand : ICommand<int>
{
    public required ChatType Type { get; init; }
    public ChatGroupDto? GroupDetails { get; init; }
    public int? RecipientId { get; init; } 
}