using RChat.Application.Accounts.Dtos;
using RChat.Application.Users.CommonDtos;

namespace RChat.Application.Abstractions.Services.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(int accountId, string roleName);
}