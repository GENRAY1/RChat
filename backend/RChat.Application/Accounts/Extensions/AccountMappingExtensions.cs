using RChat.Application.Accounts.Dtos;
using RChat.Application.Users.Extensions;
using RChat.Domain.Accounts;

namespace RChat.Application.Accounts.Extensions;

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