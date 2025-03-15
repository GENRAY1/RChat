using RChat.Application.Abstractions.Messaging;
using RChat.Application.Users.CommonDtos;
using RChat.Domain.Common;

namespace RChat.Application.Users.GetList;

public class GetUsersQuery : IQuery<List<UserDto>>
{
    public PaginationDto? Pagination { get; init; }
}