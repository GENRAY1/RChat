using RChat.Application.Common;
using RChat.Application.Common.Sorting;

namespace RChat.Domain.Members.Repository;

public class GetMemberListParameters
{
    public int[]? ChatIds { get; init; }
    public int[]? UserIds { get; init; }
    public SortingDto<MemberSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}