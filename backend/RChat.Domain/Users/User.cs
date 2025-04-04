using RChat.Domain.Abstractions;

namespace RChat.Domain.Users;

public class User : Entity
{
    public const int MaxUsernameLength = 32;
    public const int MaxDescriptionLength = 80;
    
    public const int MaxFirstnameLength = 64;
    public const int MaxLastnameLength = 64;
    
    public required int AccountId { get; init; }
    
    public required string Firstname { get; set; }
    
    public string? Lastname { get; set; } 
    
    public string? Username { get; set; }

    public string? Description { get; set; }
    
    public DateTime? DateOfBirth { get; set; }

    public required DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; } 
    
    public string FullName => $"{Firstname} {Lastname}".Trim();
} 