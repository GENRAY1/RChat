using RChat.Application.Dtos.Chats;

namespace RChat.Web.Controllers.Chats.Update;

public class UpdateChatRequest
{
    public ChatGroupDto? GroupDetails { get; init; }
}