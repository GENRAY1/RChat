namespace RChat.Web.Hubs.Chats;

public static class ChatHubEvents
{
    public const string ReceiveMessage = nameof(ReceiveMessage);
    public const string MessageUpdated = nameof(MessageUpdated);
    public const string MessageDeleted = nameof(MessageDeleted);
}