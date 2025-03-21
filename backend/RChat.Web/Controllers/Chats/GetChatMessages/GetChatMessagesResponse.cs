using RChat.Application.Messages.Dtos;

namespace RChat.Web.Controllers.Chats.GetChatMessages;

public class GetChatMessagesResponse
{
    public required List<MessageDto> Messages { get; set; }
}