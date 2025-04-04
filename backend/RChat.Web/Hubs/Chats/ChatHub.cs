using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RChat.Application.Chats.CheckAccess;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.GetCurrentUserChats;

namespace RChat.Web.Hubs.Chats;

[Authorize]
public class ChatHub(ISender sender) : Hub
{
    public async Task JoinChat(int chatId)
    {
        var checkAccessCommand =
            new CheckChatAccessBeforeJoinCommand(chatId);
        
        await sender.Send(checkAccessCommand);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,  chatId.ToString());
    }
    
    public override async Task OnConnectedAsync()
    {
        List<CurrentUserChatDto> userChats = await sender.Send(new GetCurrentUserChatsQuery());

        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }

        await base.OnConnectedAsync();
    }
}