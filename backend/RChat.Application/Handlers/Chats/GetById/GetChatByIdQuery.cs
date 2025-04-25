using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Chats;

namespace RChat.Application.Handlers.Chats.GetById;

public class GetChatByIdQuery : IQuery<ChatDto>
{
    public required int ChatId { get; init; }
}