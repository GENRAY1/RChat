namespace RChat.Domain.Users.Repository;

public class GetUserParameters
{
    public int? Id { get; init; }
    
    public string? Username { get; init; }
    
    public string? Login { get; init; }
}