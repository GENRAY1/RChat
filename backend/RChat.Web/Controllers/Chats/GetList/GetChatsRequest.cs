namespace RChat.Web.Controllers.Chats.GetList;

public class GetChatsRequest
{
    public required int Skip { get; init; }

    public required int Take { get; init; }
}