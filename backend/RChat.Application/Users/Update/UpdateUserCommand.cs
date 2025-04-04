using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Users.Update;

public class UpdateUserCommand : ICommand
{
    public required int Id { get; init; }
    
    public string? Username { get; init; }
    
    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
    
    public required string Firstname { get; init; }
    
    public string? Lastname { get; init; } 
}
