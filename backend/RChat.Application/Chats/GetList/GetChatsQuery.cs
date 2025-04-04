using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;
using RChat.Domain.Common;

namespace RChat.Application.Chats.GetList;

public class GetChatsQuery : IQuery<List<ChatDto>>
{
    public ChatType? Type { get; init; }
    public SortingDto<ChatSortingColumn>? Sorting { get; init; }
    public required PaginationDto Pagination { get; init; }
}