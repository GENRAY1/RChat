using RChat.Domain.Abstractions;

namespace RChat.Domain.Users;

public class UserRole : Entity
{
    public const int MaxNameLength = 32;
    public const int MaxDescriptionLength = 80;
    
    public static UserRole Admin = new() { Id = 1, Name = "Admin" };
    public static UserRole User = new() { Id = 2, Name = "User" };
    
    public required string Name { get; init; }
    public string? Description { get; init; }
} 