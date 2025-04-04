namespace RChat.Web.Controllers.Account.Login;

public class LoginRequest
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}