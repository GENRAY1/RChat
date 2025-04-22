namespace RChat.Application.Services.Search.Message;

public interface IMessageSearchService
{
    Task<bool> IndexAsync(int id, MessageDocument messageDocument, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(int id, UpdateMessageDocument messageDocument, CancellationToken cancellationToken);
}