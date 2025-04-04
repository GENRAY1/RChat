namespace RChat.Web.Controllers.Account.Register;

public class RegisterRequest
{
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}