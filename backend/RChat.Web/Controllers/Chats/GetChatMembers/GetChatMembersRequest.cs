using RChat.Domain.Common;
using RChat.Domain.Members;

namespace RChat.Web.Controllers.Chats.GetChatMembers;

public class GetChatMembersRequest
{
    public SortingDto<MemberSortingColumn>? Sorting { get; init; }
    
    public int Skip { get; init; }
    
    public int Take { get; init; }
}