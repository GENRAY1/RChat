using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Members.Delete;

public class DeleteChatMemberCommand : ICommand
{
    public required int ChatId { get; init; }
    
    public required int UserId { get; init; }
}