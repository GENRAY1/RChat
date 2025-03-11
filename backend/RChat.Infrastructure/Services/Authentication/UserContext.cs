using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;

namespace RChat.Infrastructure.Services.Authentication;

public class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    public int UserId
    {
        get
        {
            var value = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (value != null)
                return Int32.Parse(value);
            
            throw new UnauthenticatedException();
        }
    }
}