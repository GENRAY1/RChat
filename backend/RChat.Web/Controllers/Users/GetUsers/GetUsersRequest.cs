using RChat.Domain.Common;

namespace RChat.Web.Controllers.Users.GetUsers;

public class GetUsersRequest
{
    public required int Skip { get; init; }

    public required int Take { get; init; }
}