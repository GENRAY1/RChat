namespace RChat.Application.Services.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(int accountId, string roleName);
}