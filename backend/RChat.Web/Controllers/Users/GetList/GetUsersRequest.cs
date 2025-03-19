using RChat.Domain.Common;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Web.Controllers.Users.GetList;

public class GetUsersRequest
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    
    public required int Skip { get; init; }

    public required int Take { get; init; }
}