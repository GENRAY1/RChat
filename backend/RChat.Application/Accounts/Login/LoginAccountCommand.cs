using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Accounts.Login;

public class LoginAccountCommand : ICommand<string>
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}