namespace RChat.Web.Controllers.Chats.GetChatMessages;

public class GetChatMessagesRequest
{
    public int Skip { get; init; }
    
    public int Take { get; init; }
}