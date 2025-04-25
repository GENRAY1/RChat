using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Handlers.Chats.GetUserChats;

public class GetUserChatsQuery : IQuery<List<UserChatDto>>
{
    public int? SecondUserId { get; init; }
    public int[]? ChatIds { get; init; }
    public ChatType? Type { get; init; }
    public PaginationDto? Pagination { get; init; }
}