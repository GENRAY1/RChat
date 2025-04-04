using RChat.Domain.Abstractions;
using RChat.Domain.Users;

namespace RChat.Domain.Accounts;

public class Account : Entity
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
    
    public required int RoleId { get; set; }
    
    public required DateTime CreatedAt { get; init; }
    
    public AccountRole AccountRole { get; set; } = null!;
    
    public User? User { get; set; }
}