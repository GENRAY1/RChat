using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Chats.CheckAccess;

public class CheckChatAccessBeforeJoinCommand(int chatId) 
    : ICommand
{
    public int ChatId { get; init; } = chatId;
}