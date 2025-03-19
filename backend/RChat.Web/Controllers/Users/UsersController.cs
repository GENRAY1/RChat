using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Chats.GetListForUser;
using RChat.Application.Users.CommonDtos;
using RChat.Application.Users.GetById;
using RChat.Application.Users.GetList;
using RChat.Application.Users.Update;
using RChat.Domain.Common;
using RChat.Web.Controllers.Users.GetById;
using RChat.Web.Controllers.Users.GetList;
using RChat.Web.Controllers.Users.GetMe;
using RChat.Web.Controllers.Users.GetMeChats;
using RChat.Web.Controllers.Users.Update;

namespace RChat.Web.Controllers.Users;

[Route("api/[controller]/")]
[ApiController]
public class UsersController(
    ISender sender,
    IUserContext userContext) 
    : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<GetUserMeResponse>> GetMe(
        CancellationToken cancellationToken)
    {
        int userId = userContext.UserId;

        UserDto user = await sender.Send(new GetUserByIdQuery
            {
                Id = userId
            }, cancellationToken);
        
        return Ok(new GetUserMeResponse
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
    
    [Authorize]
    [HttpGet("me/chats")]
    public async Task<ActionResult<GetUserMeResponse>> GetMeChats(
        CancellationToken cancellationToken)
    {
        
        int userId = userContext.UserId;

        var chats = await sender.Send(new GetChatsForUserQuery
        {
            UserId = userId
        }, cancellationToken);
        
        return Ok(new GetMeChatsResponse
        {
            Chats = chats
        });
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetUserMeResponse>> GetById(
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        UserDto user = await sender.Send(
            new GetUserByIdQuery
            {
                Id = id
            }, cancellationToken);
        
        return Ok(new GetUserByIdResponse
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            DateOfBirth = user.DateOfBirth,
            Description = user.Description,
        });
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUserMeResponse>> GetList(
        [FromQuery] GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        List<UserDto> users = await sender.Send(
            new GetUsersQuery
            {
                Sorting = request.Sorting,
                Pagination = new PaginationDto
                {
                    Skip = request.Skip,
                    Take = request.Take
                }
            }, cancellationToken);
        
        return Ok(new GetUsersResponse
        {
            Users = users
        });
    }
    
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserCommand
        {
            Id = id,
            Description = request.Description,
            DateOfBirth = request.DateOfBirth,
            Username = request.Username
        }, cancellationToken);
        
        return Ok();
    }
}