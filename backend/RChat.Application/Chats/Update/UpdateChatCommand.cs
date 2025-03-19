using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;

namespace RChat.Application.Chats.Update;

public class UpdateChatCommand : ICommand
{
    public required int ChatId { get; init; }
    
    public ChatGroupDto? GroupDetails { get; init; }
}