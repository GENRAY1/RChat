using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RChat.Application.Dtos.Chats;
using RChat.Application.Handlers.Chats.CheckAccess;
using RChat.Application.Handlers.Chats.GetUserChats;

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
        List<UserChatDto> userChats = await sender.Send(new GetUserChatsQuery());

        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
        }

        await base.OnConnectedAsync();
    }
}