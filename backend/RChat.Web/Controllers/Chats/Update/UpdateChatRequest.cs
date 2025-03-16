using RChat.Application.Chats.Dtos;

namespace RChat.Web.Controllers.Chats.Update;

public class UpdateChatRequest
{
    public ChatGroupDto? GroupDetails { get; init; }
}