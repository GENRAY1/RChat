using RChat.Application.Users.CommonDtos;

namespace RChat.Web.Controllers.Users.GetUsers;

public class GetUsersResponse
{
    public required IReadOnlyCollection<UserDto> Users { get; set; }
}