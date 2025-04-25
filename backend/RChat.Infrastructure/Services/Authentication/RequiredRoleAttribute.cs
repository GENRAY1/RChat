using Microsoft.AspNetCore.Authorization;
using RChat.Domain.Accounts;

namespace RChat.Infrastructure.Services.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class RequiredRoleAttribute : AuthorizeAttribute
{
    public RequiredRoleAttribute(params string[] roleNames)
    {
        Roles = string.Join(",", roleNames);
    }
} 