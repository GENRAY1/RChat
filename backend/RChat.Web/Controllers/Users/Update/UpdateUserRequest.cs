namespace RChat.Web.Controllers.Users.Update;

public class UpdateUserRequest
{
    public string? Username { get; init; }
    
    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
    
    public required string Firstname { get; init; }
    
    public string? Lastname { get; init; } 
}