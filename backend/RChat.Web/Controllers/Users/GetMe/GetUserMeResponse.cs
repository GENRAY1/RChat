using RChat.Application.Users.CommonDtos;

namespace RChat.Web.Controllers.Users.GetMe;

public class GetUserMeResponse
{
    public required int Id { get; init; } 
    
    public required string Login { get; init; }
    
    public required string Username { get; init; }

    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }

    public required DateTime CreatedAt { get; init; }
    
    public required RoleDto Role { get; init; }
}