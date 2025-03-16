using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Chats.SoftDelete;

public class SoftDeleteChatCommand : ICommand
{
    public required int ChatId { get; init; }
}