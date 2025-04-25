using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Handlers.Accounts.Register;

public record RegisterAccountCommand : ICommand<int> 
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}