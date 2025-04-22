namespace RChat.Application.Services.Search.Chat;

public interface IChatSearchService
{
    Task<bool> IndexAsync(int id, ChatDocument chatDocument, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, UpdateChatDocument chatDocument, CancellationToken cancellationToken);
}