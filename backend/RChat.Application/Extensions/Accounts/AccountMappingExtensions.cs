using RChat.Application.Dtos.Accounts;
using RChat.Application.Extensions.Users;
using RChat.Domain.Accounts;

namespace RChat.Application.Extensions.Accounts;

public static class AccountMappingExtensions
{
    public static AccountDto MappingToDto(this Account account) => new AccountDto
    {
        Id = account.Id,
        Login = account.Login,
        Password = account.Password,
        CreatedAt = account.CreatedAt,
        User = account.User?.MappingToDto(),
        Role = account.AccountRole.MappingToDto()
    };
}