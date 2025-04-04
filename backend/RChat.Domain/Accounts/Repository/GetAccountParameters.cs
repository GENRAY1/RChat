namespace RChat.Domain.Accounts.Repository;

public class GetAccountParameters
{
    public int? Id { get; init; }
    
    public string? Login { get; init; }
}