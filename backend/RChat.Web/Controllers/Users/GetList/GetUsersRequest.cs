using RChat.Application.Common.Sorting;
using RChat.Domain.Users;

namespace RChat.Web.Controllers.Users.GetList;

public class GetUsersRequest
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    
    public required int Skip { get; init; }

    public required int Take { get; init; }
}