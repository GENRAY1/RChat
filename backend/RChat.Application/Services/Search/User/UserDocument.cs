using System.Text.Json.Serialization;

namespace RChat.Application.Services.Search.User;

public class UserDocument
{
    public string Firstname { get; init; } = default!;
    public string? Lastname { get; init; }
    public string? Username { get; init; }
}