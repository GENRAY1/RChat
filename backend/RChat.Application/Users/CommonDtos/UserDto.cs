namespace RChat.Application.Users.CommonDtos;

public record UserDto
{
    public required int Id { get; init; } 
    
    public required string Login { get; init; }
    
    public required string Password { get; init; }
    
    public required string Username { get; init; }

    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }

    public required DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; } 
    
    public required RoleDto Role { get; init; }
}