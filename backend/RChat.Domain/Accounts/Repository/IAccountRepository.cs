namespace RChat.Domain.Accounts.Repository;

public interface IAccountRepository
{
    public Task<Account?> GetAsync(GetAccountParameters parameters);
    
    public Task<int> CreateAsync(Account account);
}