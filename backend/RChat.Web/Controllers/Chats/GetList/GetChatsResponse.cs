using RChat.Application.Dtos.Chats;

namespace RChat.Web.Controllers.Chats.GetList;

public class GetChatsResponse
{
    public required List<ChatDto> Chats { get; set; }
}