using RChat.Domain.Users;

namespace RChat.Application.Services.Authentication;

public interface IAuthContext
{
    int AccountId { get; }
    
    string Role { get; }
    
    Task<User> GetUserAsync(); 
}