using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RChat.Application.Dtos.Messages;
using RChat.Application.Extensions.Messages;
using RChat.Application.Handlers.Messages.Create;
using RChat.Application.Handlers.Messages.SoftDelete;
using RChat.Application.Handlers.Messages.Update;
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
    public async Task<ActionResult<UpdateMessageDtoResponse>> Update(
        [FromRoute]int messageId,
        [FromBody]UpdateMessageRequest request,
        CancellationToken cancellationToken)
    {
        UpdateMessageDtoResponse updatedMessage = await sender.Send(
            new UpdateMessageCommand
            {
                MessageId = messageId,
                Text = request.Text
            }, cancellationToken);
        
        await chatHub.Clients
            .Group(updatedMessage.ChatId.ToString())
            .SendAsync(ChatHubEvents.MessageUpdated, updatedMessage, cancellationToken);
        
        return Ok(updatedMessage);
    }
    
    [HttpDelete]
    [Route("{messageId:int}")]
    public async Task<ActionResult<DeleteMessageDtoResponse>> Delete(
        [FromRoute]int messageId,
        CancellationToken cancellationToken)
    {
        DeleteMessageDtoResponse deletedMessage = await sender.Send(
            new SoftDeleteMessageCommand
            {
                MessageId = messageId,
            }, cancellationToken);
        
        await chatHub.Clients
            .Group(deletedMessage.ChatId.ToString())
            .SendAsync(ChatHubEvents.MessageDeleted, deletedMessage, cancellationToken);
        
        return Ok(deletedMessage);
    }
}