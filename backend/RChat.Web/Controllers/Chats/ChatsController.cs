using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Chats.Create;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.GetById;
using RChat.Application.Chats.GetList;
using RChat.Application.Chats.SoftDelete;
using RChat.Application.Chats.Update;
using RChat.Domain.Common;
using RChat.Web.Controllers.Chats.Create;
using RChat.Web.Controllers.Chats.GetById;
using RChat.Web.Controllers.Chats.GetList;
using RChat.Web.Controllers.Chats.Update;

namespace RChat.Web.Controllers.Chats;

[ApiController]
[Route("api/[controller]/")]
public class ChatsController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateChatResponse>> CreateChat(
        [FromBody] CreateChatRequest request,
        CancellationToken cancellationToken)
    {
        var chatId = await sender.Send(new CreateChatCommand
        {
            CreatorId = request.CreatorId,
            RecipientId = request.RecipientId,
            Type = request.Type,
            GroupDetails = request.GroupChat
        }, cancellationToken);

        return Ok(new CreateChatResponse
        {
            Id = chatId
        });
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetChatsResponse>> GetList(
        [FromQuery] GetChatsRequest request,
        CancellationToken cancellationToken)
    {
        List<ChatDto> chats = await sender.Send(new GetChatsQuery()
        {
            Pagination = new PaginationDto
            {
                Skip = request.Skip,
                Take = request.Take
            }
        }, cancellationToken);

        return Ok(new GetChatsResponse
        {
            Chats = chats
        });
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetChatByIdResponse>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        ChatDto chat = await sender.Send(new GetChatByIdQuery
        {
            ChatId = id
        }, cancellationToken);

        return Ok(new GetChatByIdResponse
        {
            Id = chat.Id,
            CreatedAt = chat.CreatedAt,
            CreatorId = chat.CreatorId,
            DeletedAt = chat.DeletedAt,
            GroupChat = chat.GroupChat,
            Type = chat.Type
        });
    }

    [Authorize]
    [HttpPut(("{id:int}"))]
    public async Task<ActionResult<GetChatByIdResponse>> Update(
        [FromRoute] int id,
        [FromBody] UpdateChatRequest request,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateChatCommand
        {
            ChatId = id,
            GroupDetails = request.GroupDetails
        }, cancellationToken);
        
        return Ok();
    }
    
    [Authorize]
    [HttpPatch(("{id:int}/soft-delete"))]
    public async Task<ActionResult<GetChatByIdResponse>> SoftDelete(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        await sender.Send(new SoftDeleteChatCommand
        {
            ChatId = id
        }, cancellationToken);
        
        return Ok();
    }
    
}