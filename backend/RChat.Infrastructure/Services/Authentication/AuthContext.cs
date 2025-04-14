using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Users;

namespace RChat.Infrastructure.Services.Authentication;

public class AuthContext(
    IHttpContextAccessor contextAccessor,
    IAccountRepository accountRepository
    ) : IAuthContext
{
    public int AccountId
    {
        get
        {
            var value = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (value != null)
                return Int32.Parse(value);
            
            throw new UnauthenticatedException();
        }
    }

    public string Role
    {
        get
        {
            var value = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (value != null)
                return value;
            
            throw new UnauthenticatedException();
        }
    }

    public async Task<User> GetUserAsync()
    {
        Account? account =
            await accountRepository.GetAsync(new GetAccountParameters { Id = AccountId });
        
        if (account is null)
            throw new UnauthenticatedException();
        
        if(account.User is null)
            throw new IncompleteUserRegistrationException();
        
        return account.User;
    }
}