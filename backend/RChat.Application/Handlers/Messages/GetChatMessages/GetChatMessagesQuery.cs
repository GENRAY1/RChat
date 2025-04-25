using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common;
using RChat.Application.Dtos.Messages;

namespace RChat.Application.Handlers.Messages.GetChatMessages;

public class GetChatMessagesQuery : IQuery<List<MessageDto>>
{
    public required int ChatId { get; init; }
    public required PaginationDto Pagination { get; init; }
}