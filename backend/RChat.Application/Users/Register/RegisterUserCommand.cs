using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Users.Register;

public record RegisterUserCommand : ICommand<int> 
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}