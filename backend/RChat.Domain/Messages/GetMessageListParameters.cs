using RChat.Domain.Common;

namespace RChat.Domain.Messages;

public class GetMessageListParameters
{
    public PaginationDto? Pagination { get; set; }
}