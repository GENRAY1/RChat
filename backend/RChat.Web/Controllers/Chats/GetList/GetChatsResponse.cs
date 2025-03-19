using RChat.Application.Chats.Dtos;

namespace RChat.Web.Controllers.Chats.GetList;

public class GetChatsResponse
{
    public required List<ChatDto> Chats { get; set; }
}