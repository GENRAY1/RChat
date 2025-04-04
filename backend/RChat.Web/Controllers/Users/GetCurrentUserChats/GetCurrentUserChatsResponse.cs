using RChat.Application.Chats.Dtos;

namespace RChat.Web.Controllers.Users.GetCurrentUserChats;

public class GetCurrentUserChatsResponse
{
    public required List<CurrentUserChatDto> Chats { get; set; }
}