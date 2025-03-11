using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Users.GetById;
using RChat.Application.Users.Login;
using RChat.Application.Users.Register;
using RChat.Web.Controllers.Contracts;

namespace RChat.Web.Controllers;

[ApiController]
[Route("api/")]
public class AuthController(
    ISender sender,
    IUserContext userContext) 
    : ControllerBase
{
    [HttpPost("login")]
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
 
    [HttpPost("register")]
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

    [Authorize]
    [HttpGet ("me")]
    public async Task<ActionResult> Me(CancellationToken cancellationToken)
    {
        int userId = userContext.UserId;

        var user = await sender.Send(
            new GetUserByIdQuery
            {
                Id = userId
            }, cancellationToken);
        
        return Ok(new MeResponse
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            DateOfBirth = user.DateOfBirth,
            Description = user.Description,
            Login = user.Login
        });
    }
}