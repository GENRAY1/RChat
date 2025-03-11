using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Users.Login;

public class LoginUserCommand : ICommand<string>
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}