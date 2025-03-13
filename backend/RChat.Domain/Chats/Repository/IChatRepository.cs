namespace RChat.Domain.Chats.Repository;

public interface IChatRepository
{
    public Task<Chat?> GetByIdAsync(int chatId);
    
    public Task<List<Chat>> GetListAsync(GetChatListParameters parameters);
    
    public Task<int> CreateAsync(Chat chat);
    
    public Task UpdateAsync(Chat chat);
}