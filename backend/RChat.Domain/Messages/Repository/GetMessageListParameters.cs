using RChat.Domain.Common;

namespace RChat.Domain.Messages.Repository;

public class GetMessageListParameters
{
    public int? ChatId { get; init; }
    public SortingDto<MessageSortingColumn>? Sorting { get; init; }
    public PaginationDto? Pagination { get; init; }
}