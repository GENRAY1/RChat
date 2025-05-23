using RChat.Application.Dtos.Users;

namespace RChat.Application.Dtos.Accounts;

public class AccountDto
{
    public int Id { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
    public DateTime CreatedAt { get; init; }
    public UserDto? User { get; init; }
    public required AccountRoleDto Role { get; init; }
}