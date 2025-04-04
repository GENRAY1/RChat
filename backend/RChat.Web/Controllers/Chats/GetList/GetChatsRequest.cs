using RChat.Domain.Chats;
using RChat.Domain.Common;

namespace RChat.Web.Controllers.Chats.GetList;

public class GetChatsRequest
{
    public ChatType? Type { get; init; }
    
    public SortingDto<ChatSortingColumn>? Sorting { get; init; }
    
    public required int Skip { get; init; }

    public required int Take { get; init; }
}