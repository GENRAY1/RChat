using RChat.Application.Accounts.Dtos;
using RChat.Domain.Accounts;

namespace RChat.Application.Accounts.Extensions;

public static class RoleMappingExtensions
{
    public static AccountRoleDto MappingToDto(this AccountRole role) => new()
    {
       Id = role.Id,
       Name = role.Name, 
       Description = role.Description
    };
}