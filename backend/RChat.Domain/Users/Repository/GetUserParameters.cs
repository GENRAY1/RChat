namespace RChat.Domain.Users.Repository;

public class GetUserParameters
{
    public int? Id { get; init; }
    public int? AccountId { get; init; }
    public string? Username { get; init; }
}