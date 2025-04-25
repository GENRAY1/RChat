using RChat.Application.Dtos.Users;

namespace RChat.Web.Controllers.Users.GetList;

public class GetUsersResponse
{
    public required IReadOnlyCollection<UserDto> Users { get; set; }
}