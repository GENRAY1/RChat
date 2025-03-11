namespace RChat.Infrastructure.Services.Authentication;

public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required long LifeTimeInMinutes { get; init; }
    public required string Key { get; init; }
}