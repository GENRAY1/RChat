using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;

namespace RChat.Application.Chats.GetById;

public class GetChatByIdQuery : IQuery<ChatDto>
{
    public required int ChatId { get; set; }
}