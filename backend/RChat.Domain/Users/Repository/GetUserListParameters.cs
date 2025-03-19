using RChat.Domain.Common;

namespace RChat.Domain.Users.Repository;

public class GetUserListParameters
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}