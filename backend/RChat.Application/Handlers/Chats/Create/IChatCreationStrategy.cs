namespace RChat.Application.Handlers.Chats.Create;

public interface IChatCreationStrategy
{
    Task<int> CreateChatAsync(CreateChatCommand request);
}