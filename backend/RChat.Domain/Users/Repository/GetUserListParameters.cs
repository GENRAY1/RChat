using RChat.Application.Common;
using RChat.Application.Common.Sorting;

namespace RChat.Domain.Users.Repository;

public class GetUserListParameters
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}