namespace RChat.Web.Controllers.Auth.Login;

public class LoginRequest
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}