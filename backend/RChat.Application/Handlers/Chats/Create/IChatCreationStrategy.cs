namespace RChat.Application.Handlers.Chats.Create;

public interface IChatCreationStrategy
{
    Task<CreateChatDtoResponse> CreateChatAsync(CreateChatCommand request);
}