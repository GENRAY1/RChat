using RChat.Domain.Abstractions;

namespace RChat.Domain.Accounts;

public class AccountRole : Entity
{
    public const int MaxNameLength = 32;
    public const int MaxDescriptionLength = 80;
    
    public static AccountRole Admin = new() { Id = 1, Name = "Admin" };
    public static AccountRole User = new() { Id = 2, Name = "User" };
    
    public required string Name { get; init; }
    public string? Description { get; init; }
} 