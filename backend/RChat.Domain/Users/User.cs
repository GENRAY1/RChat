using RChat.Domain.Abstractions;

namespace RChat.Domain.Users;

public class User : Entity
{
    public const int MaxUsernameLength = 32;
    public const int MaxDescriptionLength = 80;
    
    public required string Login { get; init; }
    
    public required string Password { get; set; }
    
    public string Username { get; set; } = null!;

    public string? Description { get; set; }
    
    public DateTime? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; } 
    
    public required int RoleId { get; set; }
    public UserRole UserRole { get; set; } = null!;
} 