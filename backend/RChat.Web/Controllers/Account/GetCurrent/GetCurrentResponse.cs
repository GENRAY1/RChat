using RChat.Application.Dtos.Accounts;
using RChat.Application.Dtos.Users;

namespace RChat.Web.Controllers.Account.GetCurrent;

public class GetCurrentResponse
{
    public int Id { get; init; }
    
    public UserDto? User { get; init; }
    
    public required AccountRoleDto Role { get; init; }
}