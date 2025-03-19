using RChat.Domain.Common;

namespace RChat.Domain.Chats.Repository;

public class GetChatListParameters
{
    public int[]? UserIds { get; init; }
    public int[]? ChatIds { get; init; }
    public ChatType? Type { get; init; }
    
    public bool? OnlyActive { get; init; }
    public PaginationDto? Pagination { get; set; }
}