using RChat.Domain.Abstractions;

namespace RChat.Domain.Accounts;

public class AccountRole : Entity
{
    public const int MaxNameLength = 32;
    public const int MaxDescriptionLength = 80;
    
    public static readonly AccountRole Admin = new() { Id = 1, Name = AccountRoleNames.Admin };
    public static readonly AccountRole User = new() { Id = 2, Name = AccountRoleNames.User };
    
    public required string Name { get; init; }
    public string? Description { get; init; }
} 