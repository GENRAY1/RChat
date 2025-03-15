using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Users.Login;
using RChat.Application.Users.Register;
using RChat.Web.Controllers.Auth.Login;
using RChat.Web.Controllers.Auth.Register;

namespace RChat.Web.Controllers.Auth;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(ISender sender) 
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand()
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
    [HttpPost]
    public async Task<ActionResult<RegisterResponse>> Register(
        [FromBody]RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
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
}