using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RChat.Application.Messages.Create;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.SoftDelete;
using RChat.Application.Messages.Update;
using RChat.Web.Controllers.Messages.Create;
using RChat.Web.Controllers.Messages.Delete;
using RChat.Web.Controllers.Messages.Update;
using RChat.Web.Hubs.Chats;

namespace RChat.Web.Controllers.Messages;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(
    ISender sender,
    IHubContext<ChatHub> chatHub
    ) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> Create(
        CreateMessageRequest request,
        CancellationToken cancellationToken)
    {
        MessageDto message = await sender.Send(
            new CreateMessageCommand
            {
                ChatId = request.ChatId,
                Text = request.Text,
                ReplyToMessageId = request.ReplyToMessageId
            }, cancellationToken);

        await chatHub.Clients
            .Group(request.ChatId.ToString())
            .SendAsync(ChatHubEvents.ReceiveMessage, message, cancellationToken);

        return Ok(message);
    }
    
    [HttpPut]
    [Route("{messageId:int}")]
    public async Task<ActionResult<UpdateMessageResponse>> Update(
        [FromRoute]int messageId,
        [FromBody]UpdateMessageRequest request,
        CancellationToken cancellationToken)
    {
        MessageDto message = await sender.Send(
            new UpdateMessageCommand
            {
                MessageId = messageId,
                Text = request.Text
            }, cancellationToken);
        
        var updateMessageResponse = new UpdateMessageResponse
        {
            UpdatedAt = message.UpdatedAt!.Value,
            Text = message.Text,
            MessageId = message.Id,
        };

        await chatHub.Clients
            .Group(message.ChatId.ToString())
            .SendAsync(ChatHubEvents.MessageUpdated, updateMessageResponse, cancellationToken);
        
        return Ok(updateMessageResponse);
    }
    
    [HttpDelete]
    [Route("{messageId:int}")]
    public async Task<ActionResult<DeleteMessageResponse>> Delete(
        [FromRoute]int messageId,
        CancellationToken cancellationToken)
    {
        MessageDto message = await sender.Send(
            new SoftDeleteMessageCommand
            {
                MessageId = messageId,
            }, cancellationToken);

        var deleteMessageResponse = new DeleteMessageResponse
        {
            DeletedAt = message.DeletedAt!.Value,
            MessageId = message.Id,
        };

        await chatHub.Clients
            .Group(message.ChatId.ToString())
            .SendAsync(ChatHubEvents.MessageDeleted, deleteMessageResponse, cancellationToken);
        
        return Ok(deleteMessageResponse);
    }
}