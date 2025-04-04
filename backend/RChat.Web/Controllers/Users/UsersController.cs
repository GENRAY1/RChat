using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Chats.GetCurrentUserChats;
using RChat.Application.Users.CommonDtos;
using RChat.Application.Users.Create;
using RChat.Application.Users.GetById;
using RChat.Application.Users.GetList;
using RChat.Application.Users.Update;
using RChat.Domain.Common;
using RChat.Web.Controllers.Users.Create;
using RChat.Web.Controllers.Users.GetCurrentUserChats;
using RChat.Web.Controllers.Users.GetList;
using RChat.Web.Controllers.Users.Update;

namespace RChat.Web.Controllers.Users;

[Route("api/[controller]/")]
[ApiController]
public class UsersController(ISender sender) 
    : ControllerBase
{
    [Authorize]
    [HttpGet("me/chats")]
    public async Task<ActionResult<GetCurrentUserChatsResponse>> GetCurrentUserChats(
        CancellationToken cancellationToken)
    {
        var chats =
            await sender.Send(new GetCurrentUserChatsQuery(), cancellationToken);
        
        return Ok(new GetCurrentUserChatsResponse
        {
            Chats = chats
        });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        UserDto user = await sender.Send(new CreateUserCommand {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Username = request.Username,
            Description  = request.Description,
            DateOfBirth = request.DateOfBirth
        }, cancellationToken);
        
        return Ok(user);
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        UserDto user = await sender.Send(
            new GetUserByIdQuery
            {
                Id = id
            }, cancellationToken);
        
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUsersResponse>> GetList(
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
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname
        }, cancellationToken);
        
        return Ok();
    }
}