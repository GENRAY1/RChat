using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;
using RChat.Domain.Common;
using RChat.Domain.Messages;

namespace RChat.Application.Messages.GetChatMessages;

public class GetChatMessagesQuery : IQuery<List<MessageDto>>
{
    public required int ChatId { get; init; }
    public required PaginationDto Pagination { get; init; }
}