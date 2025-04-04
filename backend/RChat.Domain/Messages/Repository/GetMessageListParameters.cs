using RChat.Domain.Common;

namespace RChat.Domain.Messages.Repository;

public class GetMessageListParameters
{
    public int? ChatId { get; set; }
    
    public bool? OnlyActive { get; set; }
    public SortingDto<MessageSortingColumn>? Sorting { get; set; }
    public PaginationDto? Pagination { get; set; }
}