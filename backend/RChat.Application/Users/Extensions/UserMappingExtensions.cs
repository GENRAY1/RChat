using RChat.Application.Users.CommonDtos;
using RChat.Domain.Users;

namespace RChat.Application.Users.Extensions;

public static class UserMappingExtensions
{
    public static UserDto MappingToDto(this User user) => new UserDto
    {
        Id = user.Id,
        Login = user.Login,
        Username = user.Username,
        DateOfBirth = user.DateOfBirth,
        Password = user.Password,
        Description = user.Description,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        Role = user.UserRole.MappingToDto()
    };
}