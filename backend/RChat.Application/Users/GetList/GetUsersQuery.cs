using RChat.Application.Abstractions.Messaging;
using RChat.Application.Users.CommonDtos;
using RChat.Domain.Common;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Users.GetList;

public class GetUsersQuery : IQuery<List<UserDto>>
{
    public SortingDto<UserSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}