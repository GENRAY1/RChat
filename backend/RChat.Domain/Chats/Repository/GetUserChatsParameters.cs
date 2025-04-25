using RChat.Application.Common;

namespace RChat.Domain.Chats.Repository;

public class GetUserChatsParameters
{
    public required int UserId { get; init; }
    public int? SecondUserId { get; init; }
    public int[]? ChatIds { get; init; }
    public ChatType? Type { get; init; }
    public PaginationDto? Pagination { get; init; }
}