using RChat.Application.Dtos.Chats;

namespace RChat.Web.Controllers.Users.GetUserChats;

public class GetUserChatsResponse
{
    public required List<UserChatDto> Chats { get; set; }
}