namespace RChat.Web.Controllers.Users.GetList;

public class GetUsersRequest
{
    public required int Skip { get; init; }

    public required int Take { get; init; }
}