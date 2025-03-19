using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Members.Create;

public class CreateMemberCommand : ICommand<int>
{
    public required int UserId { get; init; }
    public required int ChatId { get; init; }
}