using RChat.Domain.Common;

namespace RChat.Domain.Members;

public class GetMemberListParameters
{
    public PaginationDto? Pagination { get; set; }
}