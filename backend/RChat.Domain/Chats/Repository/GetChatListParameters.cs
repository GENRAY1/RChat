using RChat.Application.Common;
using RChat.Application.Common.Sorting;

namespace RChat.Domain.Chats.Repository;

public class GetChatListParameters
{
    public int[]? ChatIds { get; init; }
    public ChatType? Type { get; init; }
    public bool? OnlyActive { get; init; }
    public SortingDto<ChatSortingColumn>? Sorting{ get; init; }
    public PaginationDto? Pagination { get; init; }
}