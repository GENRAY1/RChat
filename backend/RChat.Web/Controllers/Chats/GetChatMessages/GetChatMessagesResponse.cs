using RChat.Application.Dtos.Messages;

namespace RChat.Web.Controllers.Chats.GetChatMessages;

public class GetChatMessagesResponse
{
    public required List<MessageDto> Messages { get; set; }
}