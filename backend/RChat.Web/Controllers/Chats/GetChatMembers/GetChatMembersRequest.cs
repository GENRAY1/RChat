namespace RChat.Web.Controllers.Chats.GetChatMembers;

public class GetChatMembersRequest
{
    public int Skip { get; init; }
    
    public int Take { get; init; }
}