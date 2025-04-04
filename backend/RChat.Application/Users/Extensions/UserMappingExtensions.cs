using RChat.Application.Users.CommonDtos;
using RChat.Domain.Users;

namespace RChat.Application.Users.Extensions;

public static class UserMappingExtensions
{
    public static UserDto MappingToDto(this User user) => new UserDto
    {
        Id = user.Id,
        AccountId = user.AccountId,
        Username = user.Username,
        DateOfBirth = user.DateOfBirth,
        Description = user.Description,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        Lastname = user.Lastname,
        Firstname = user.Firstname
    };
}