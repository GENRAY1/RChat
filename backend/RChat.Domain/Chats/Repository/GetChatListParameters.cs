using RChat.Domain.Common;

namespace RChat.Domain.Chats.Repository;

public class GetChatListParameters
{
    public PaginationDto? Pagination { get; set; }
}