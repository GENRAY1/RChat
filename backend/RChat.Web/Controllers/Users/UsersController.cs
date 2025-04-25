using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Common;
using RChat.Application.Dtos.Users;
using RChat.Application.Handlers.Chats.GetUserChats;
using RChat.Application.Handlers.Users.Create;
using RChat.Application.Handlers.Users.GetById;
using RChat.Application.Handlers.Users.GetList;
using RChat.Application.Handlers.Users.Update;
using RChat.Web.Controllers.Users.Create;
using RChat.Web.Controllers.Users.GetList;
using RChat.Web.Controllers.Users.GetUserChats;
using RChat.Web.Controllers.Users.Update;

namespace RChat.Web.Controllers.Users;

[Route("api/[controller]/")]
[ApiController]
public class UsersController(ISender sender) 
    : ControllerBase
{
    [Authorize]
    [HttpGet("me/chats")]
    public async Task<ActionResult<GetUserChatsResponse>> GetUserChats(
        [FromQuery] GetUserChatsRequest request,
        CancellationToken cancellationToken)
    {
        var chats =
            await sender.Send(new GetUserChatsQuery
            {
                SecondUserId = request.SecondUserId,
                ChatIds = request.ChatIds,
                Type = request.Type,
                Pagination = new PaginationDto
                {
                    Skip = request.Skip,
                    Take = request.Take
                }
            }, cancellationToken);
        
        return Ok(new GetUserChatsResponse
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