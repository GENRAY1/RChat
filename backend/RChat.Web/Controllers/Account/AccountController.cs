using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Dtos.Accounts;
using RChat.Application.Handlers.Accounts.GetCurrent;
using RChat.Application.Handlers.Accounts.Login;
using RChat.Application.Handlers.Accounts.Register;
using RChat.Web.Controllers.Account.GetCurrent;
using RChat.Web.Controllers.Account.Login;
using RChat.Web.Controllers.Account.Register;
using RChat.Web.Controllers.Chats.GetList;

namespace RChat.Web.Controllers.Account;

[ApiController]
[Route("api/[controller]/")]
public class AccountController(ISender sender) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginAccountCommand()
        {
            Login = request.Login,
            Password = request.Password
        };
        
        var accessToken =
            await sender.Send(command, cancellationToken);
        
        return Ok(new LoginResponse
        {
            AccessToken = accessToken
        });
    }
 
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(
        [FromBody]RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterAccountCommand
        {
            Login = request.Login,
            Password = request.Password
        };
        
        var userId =
            await sender.Send(command, cancellationToken);
        
        return Ok(new RegisterResponse
        {
            Id = userId
        });
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<GetChatsResponse>> GetCurrent(
        CancellationToken cancellationToken)
    {
        AccountDto account = 
            await sender.Send(new GetCurrentAccountQuery(), cancellationToken);
        
        return Ok(new GetCurrentResponse
        {
            Id = account.Id,
            User = account.User,
            Role = account.Role
        });
    }
}