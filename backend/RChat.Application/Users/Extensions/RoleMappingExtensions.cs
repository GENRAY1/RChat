using RChat.Application.Users.CommonDtos;
using RChat.Domain.Users;

namespace RChat.Application.Users.Extensions;

public static class RoleMappingExtensions
{
    public static RoleDto MappingToDto(this UserRole role) => new()
    {
       Id = role.Id,
       Name = role.Name, 
       Description = role.Description
    };
}