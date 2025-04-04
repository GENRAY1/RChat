namespace RChat.Application.Users.CommonDtos;

public record UserDto
{
    public required int Id { get; init; } 
    
    public required int AccountId { get; init; }
    
    public required string Firstname { get; set; }
    
    public string? Lastname { get; set; } 
    
    public string? Username { get; init; }

    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }

    public required DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; } 
}