using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Handlers.Members.Delete;

public class DeleteMemberCommand : ICommand
{
    public required int MemberId { get; init; }
}