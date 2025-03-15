using RChat.Domain.Common;

namespace RChat.Domain.Users.Repository;

public class GetUserListParameters
{
    public PaginationDto? Pagination { get; init; }
}