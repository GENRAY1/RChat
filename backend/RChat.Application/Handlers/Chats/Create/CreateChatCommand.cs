using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Handlers.Chats.Create;

public class CreateChatCommand : ICommand<CreateChatDtoResponse>
{
    public required ChatType Type { get; init; }
    public ChatGroupDto? GroupDetails { get; init; }
    public int? RecipientId { get; init; } 
}