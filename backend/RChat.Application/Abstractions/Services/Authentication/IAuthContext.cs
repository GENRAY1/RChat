using RChat.Domain.Accounts;
using RChat.Domain.Users;

namespace RChat.Application.Abstractions.Services.Authentication;

public interface IAuthContext
{
    int AccountId { get; }
    
    string Role { get; }
    
    Task<User> GetUserAsync(); 
}