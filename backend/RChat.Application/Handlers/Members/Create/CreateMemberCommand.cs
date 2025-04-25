using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Handlers.Members.Create;

public class CreateMemberCommand : ICommand<int>
{
    public required int ChatId { get; init; }
}