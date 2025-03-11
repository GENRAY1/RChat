using RChat.Application.Users.CommonDtos;

namespace RChat.Application.Abstractions.Services.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(UserDto user);
}