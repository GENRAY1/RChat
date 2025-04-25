using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Handlers.Chats.GetList;

public class GetChatsQuery : IQuery<List<ChatDto>>
{
    public ChatType? Type { get; init; }
    public SortingDto<ChatSortingColumn>? Sorting { get; init; }
    public required PaginationDto Pagination { get; init; }
}