using RChat.Domain.Common;

namespace RChat.Domain.Members;

public class GetMemberListParameters
{
    public int[]? ChatIds { get; set; } 
    
    public int[]? UserIds { get; set; }
    public PaginationDto? Pagination { get; set; }
}