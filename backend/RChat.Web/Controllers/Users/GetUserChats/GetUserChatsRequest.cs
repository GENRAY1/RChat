using RChat.Application.Common.Sorting;
using RChat.Domain.Chats;

namespace RChat.Web.Controllers.Users.GetUserChats;

public class GetUserChatsRequest
{
    public int? SecondUserId { get; init; }
    public int[]? ChatIds { get; init; }
    public ChatType? Type { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
}