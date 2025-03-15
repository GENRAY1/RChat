using RChat.Application.Users.CommonDtos;

namespace RChat.Web.Controllers.Users.GetById;

public class GetUserByIdResponse
{
    public required int Id { get; init; } 
    
    public required string Username { get; init; }

    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
    
    public required RoleDto Role { get; init; }
}