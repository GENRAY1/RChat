namespace RChat.Domain.Messages.Repository;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(int messageId);
    
    Task<List<Message>> GetListAsync(GetMessageListParameters parameters);
    
    Task<int> CreateAsync(Message message);
    
    Task UpdateAsync(Message message);
    Task DeleteAsync(int messageId);
}