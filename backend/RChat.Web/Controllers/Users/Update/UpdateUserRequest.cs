namespace RChat.Web.Controllers.Users.Update;

public class UpdateUserRequest
{
    public required string Username { get; init; }
    
    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; set; }
}