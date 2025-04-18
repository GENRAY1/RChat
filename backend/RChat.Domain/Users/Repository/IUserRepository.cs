namespace RChat.Domain.Users.Repository;

public interface IUserRepository
{
    public Task<User?> GetAsync(GetUserParameters parameters);
    
    public Task<List<User>> GetListAsync(GetUserListParameters parameters);
    
    public Task<int> CreateAsync(User user);
    
    public Task<int> UpdateAsync(User user);
}