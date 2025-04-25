namespace RChat.Application.Dtos.GlobalSearch;

public class UserSearchDto
{
    public required int Id { get; init; }
    public required string Firstname { get; init; }
    public string? Lastname { get; init; }
    public string? Username { get; init; }
}