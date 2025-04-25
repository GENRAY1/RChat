using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Users;

namespace RChat.Application.Handlers.Users.Create;

public class CreateUserCommand : ICommand<UserDto>
{
    public required string Firstname { get; init; }
    public string? Lastname { get; init; } 
    public string? Username { get; init; }
    public string? Description { get; init; }
    public DateTime? DateOfBirth { get; init; }
}