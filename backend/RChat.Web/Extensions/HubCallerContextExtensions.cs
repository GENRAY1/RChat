using Microsoft.AspNetCore.SignalR;
using RChat.Application.Exceptions;

namespace RChat.Web.Extensions;

public static class HubCallerContextExtensions
{
    public static int GetUserId(this HubCallerContext context)
    {
        if (context.UserIdentifier == null || !int.TryParse(context.UserIdentifier, out int userId))
        {
            throw new UnauthenticatedException();
        }
        
        return userId;
    }
}