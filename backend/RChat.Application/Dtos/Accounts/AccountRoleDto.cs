namespace RChat.Application.Dtos.Accounts;

public class AccountRoleDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}