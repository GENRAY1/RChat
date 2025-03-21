using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RChat.Application.Chats.CheckAccess;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.GetById;
using RChat.Application.Chats.GetListForUser;
using RChat.Application.Messages.Create;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.SoftDelete;
using RChat.Application.Messages.Update;
using RChat.Domain.Chats;
using RChat.Web.Extensions;
using RChat.Web.Hubs.Chats.DeleteMessage;
using RChat.Web.Hubs.Chats.SendMessage;
using RChat.Web.Hubs.Chats.UpdateMessage;

namespace RChat.Web.Hubs.Chats;

[Authorize]
public class ChatHub(ISender sender) 
    : Hub
{
    public async Task SendMessage(SendMessageRequest request)
    {
        MessageDto message = await sender.Send(
            new CreateMessageCommand
            {
                ChatId = request.ChatId,
                Text = request.Text,
                ReplyToMessageId = request.ReplyToMessageId
            });

        await Clients
            .Group(GetGroupName(request.ChatId))
            .SendAsync(ChatHubEvents.ReceiveMessage, message);
    }
    
    public async Task UpdateMessage(UpdateMessageRequest request)
    {
        MessageDto message = await sender.Send(
            new UpdateMessageCommand
            {
                MessageId = request.MessageId,
                Text = request.Text
            });
        
        var updateMessageResponse = new UpdateMessageResponse
        {
            UpdatedAt = message.UpdatedAt!.Value,
            Text = message.Text,
            MessageId = message.Id,
        };

        await Clients
            .Group(GetGroupName(message.ChatId))
            .SendAsync(ChatHubEvents.MessageUpdated, updateMessageResponse);
    }
    
    public async Task DeleteMessage(DeleteMessageRequest request)
    {
        MessageDto message = await sender.Send(
            new SoftDeleteMessageCommand
            {
                MessageId = request.MessageId,
            });

        var deleteMessageResponse = new DeleteMessageResponse
        {
            DeletedAt = message.DeletedAt!.Value,
            MessageId = message.Id,
        };

        await Clients
            .Group(GetGroupName(message.ChatId))
            .SendAsync(ChatHubEvents.MessageDeleted, deleteMessageResponse);
    }
    
    public async Task JoinChat(int chatId)
    {
        var checkAccessCommand =
            new CheckChatAccessBeforeJoinCommand(chatId);
        
        await sender.Send(checkAccessCommand);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(chatId));
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,  GetGroupName(chatId));
    }
    
    public override async Task OnConnectedAsync()
    {
        List<UserChatDto> userChats = await sender.Send(
            new GetChatsForUserQuery
            {
                UserId = Context.GetUserId()
            });

        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(chat.Id));
        }

        await base.OnConnectedAsync();
    }
    
    private string GetGroupName(int chatId) => $"chat_{chatId}";
}