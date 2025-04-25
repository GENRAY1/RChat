using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Chats;

namespace RChat.Application.Handlers.Chats.Update;

public class UpdateChatCommand : ICommand
{
    public required int ChatId { get; init; }
    
    public ChatGroupDto? GroupDetails { get; init; }
}