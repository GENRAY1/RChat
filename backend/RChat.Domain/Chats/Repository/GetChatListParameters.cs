using RChat.Domain.Common;

namespace RChat.Domain.Chats.Repository;

public class GetChatListParameters
{
    public int[]? UserIds { get; set; }
    public int[]? ChatIds { get; set; }
    public ChatType? Type { get; set; }

    public int? OnlyAccessibleByUserId { get; set; }
    public bool? OnlyActive { get; set; }
    public SortingDto<ChatSortingColumn>? Sorting{ get; set; }
    public PaginationDto? Pagination { get; set; }
}