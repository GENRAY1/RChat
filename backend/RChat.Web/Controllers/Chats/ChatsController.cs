using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RChat.Application.Common;
using RChat.Application.Dtos.Chats;
using RChat.Application.Dtos.Members;
using RChat.Application.Dtos.Messages;
using RChat.Application.Handlers.Chats.Create;
using RChat.Application.Handlers.Chats.GetById;
using RChat.Application.Handlers.Chats.GetList;
using RChat.Application.Handlers.Chats.SoftDelete;
using RChat.Application.Handlers.Chats.Update;
using RChat.Application.Handlers.Members.GetChatMembers;
using RChat.Application.Handlers.Messages.GetChatMessages;
using RChat.Domain.Accounts;
using RChat.Infrastructure.Services.Authentication;
using RChat.Web.Controllers.Chats.Create;
using RChat.Web.Controllers.Chats.GetById;
using RChat.Web.Controllers.Chats.GetChatMembers;
using RChat.Web.Controllers.Chats.GetChatMessages;
using RChat.Web.Controllers.Chats.GetList;
using RChat.Web.Controllers.Chats.Update;

namespace RChat.Web.Controllers.Chats;

[ApiController]
[Route("api/[controller]/")]
public class ChatsController(ISender sender)
    : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateChatResponse>> CreateChat(
        [FromBody] CreateChatRequest request,
        CancellationToken cancellationToken)
    {
        var chatId = await sender.Send(new CreateChatCommand
        {
            RecipientId = request.RecipientId,
            Type = request.Type,
            GroupDetails = request.GroupChat
        }, cancellationToken);

        return Ok(new CreateChatResponse
        {
            Id = chatId
        });
    }
    
    [HttpGet]
    [RequiredRole(AccountRoleNames.Admin)]
    public async Task<ActionResult<GetChatsResponse>> GetList(
        [FromQuery] GetChatsRequest request,
        CancellationToken cancellationToken)
    {
        List<ChatDto> chats = await sender.Send(new GetChatsQuery()
        {
            Type = request.Type,
            Sorting = request.Sorting,
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
    [HttpGet("{chatId:int}")]
    public async Task<ActionResult<GetChatByIdResponse>> GetById(
        [FromRoute] int chatId,
        CancellationToken cancellationToken)
    {
        ChatDto chat = await sender.Send(new GetChatByIdQuery
        {
            ChatId = chatId
        }, cancellationToken);

        return Ok(new GetChatByIdResponse
        {
            Id = chat.Id,
            CreatedAt = chat.CreatedAt,
            CreatorId = chat.CreatorId,
            DeletedAt = chat.DeletedAt,
            GroupChat = chat.GroupChat,
            MemberCount = chat.MemberCount,
            Type = chat.Type
        });
    }

    [Authorize]
    [HttpPut(("{chatId:int}"))]
    public async Task<ActionResult<GetChatByIdResponse>> Update(
        [FromRoute] int chatId,
        [FromBody] UpdateChatRequest request,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateChatCommand
        {
            ChatId = chatId,
            GroupDetails = request.GroupDetails
        }, cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete(("{chatId:int}"))]
    public async Task<ActionResult<GetChatByIdResponse>> SoftDelete(
        [FromRoute] int chatId,
        CancellationToken cancellationToken)
    {
        await sender.Send(new SoftDeleteChatCommand
        {
            ChatId = chatId
        }, cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpGet("{chatId:int}/members")]
    public async Task<ActionResult<GetChatMembersResponse>> GetMembers(
        [FromRoute] int chatId,
        [FromQuery] GetChatMembersRequest request,
        CancellationToken cancellationToken)
    {
        List<MemberDto> members = await sender.Send(new GetMembersQuery
        {
            Sorting = request.Sorting,
            Pagination = new PaginationDto
            {
                Skip = request.Skip,
                Take = request.Take
            },
            ChatId = chatId
        }, cancellationToken);

        return Ok(new GetChatMembersResponse
        {
            Members = members
        });
    }

    [Authorize]
    [HttpGet("{chatId:int}/messages")]
    public async Task<ActionResult<GetChatMessagesResponse>> GetMessages(
        [FromRoute] int chatId,
        [FromQuery] GetChatMessagesRequest request,
        CancellationToken cancellationToken)
    {
        List<MessageDto> messages = await sender.Send(
            new GetChatMessagesQuery
            {
                Pagination = new PaginationDto
                {
                    Skip = request.Skip,
                    Take = request.Take
                },
                ChatId = chatId
            }, cancellationToken);

        return Ok(new GetChatMessagesResponse()
        {
            Messages = messages
        });
    }
}