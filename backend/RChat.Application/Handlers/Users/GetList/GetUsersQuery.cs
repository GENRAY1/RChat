using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Users;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Users.GetList;

public class GetUsersQuery : IQuery<List<UserDto>>
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}