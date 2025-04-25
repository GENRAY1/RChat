using RChat.Application.Dtos.Accounts;
using RChat.Domain.Accounts;

namespace RChat.Application.Extensions.Accounts;

public static class RoleMappingExtensions
{
    public static AccountRoleDto MappingToDto(this AccountRole role) => new()
    {
       Id = role.Id,
       Name = role.Name, 
       Description = role.Description
    };
}