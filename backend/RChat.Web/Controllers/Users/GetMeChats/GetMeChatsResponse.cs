using RChat.Application.Chats.Dtos;

namespace RChat.Web.Controllers.Users.GetMeChats;

public class GetMeChatsResponse
{
    public required List<UserChatDto> Chats { get; set; }
}