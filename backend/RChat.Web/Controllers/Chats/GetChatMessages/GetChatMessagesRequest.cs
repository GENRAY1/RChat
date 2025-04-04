namespace RChat.Web.Controllers.Chats.GetChatMessages;

public class GetChatMessagesRequest
{
    public required int Skip { get; init; }
    
    public required int Take { get; init; }
}