using RChat.Domain.Messages;

namespace RChat.Domain.Chats;

public class UserChat : Chat{
    public Message? LatestMessage { get; set; } 
}