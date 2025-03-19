namespace RChat.Application.Chats.Create;

public interface IChatCreationStrategy
{
    Task<int> CreateChatAsync(CreateChatCommand request);
}