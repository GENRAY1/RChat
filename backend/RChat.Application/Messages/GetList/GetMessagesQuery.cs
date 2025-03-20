using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;
using RChat.Domain.Common;
using RChat.Domain.Messages;

namespace RChat.Application.Messages.GetList;

public class GetMessagesQuery : IQuery<List<MessageDto>>
{
    public int? ChatId { get; init; }
    
    public SortingDto<MessageSortingColumn>? Sorting { get; init; } 
    
    public PaginationDto? Pagination { get; init; }
}