using RChat.Domain.Common;
using RChat.Domain.Members;

namespace RChat.Web.Controllers.Chats.GetChatMembers;

public class GetChatMembersRequest
{
    public SortingDto<MemberSortingColumn>? Sorting { get; init; }
    
    public required int Skip { get; init; }
    
    public required int Take { get; init; }
}