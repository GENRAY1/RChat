namespace RChat.Web.Controllers.Users.Create;

public class CreateUserRequest
{
    public required string Firstname { get; init; }
    public string? Lastname { get; init; } 
    public string? Username { get; init; }
    public string? Description { get; init; }
    public DateTime? DateOfBirth { get; init; }
}