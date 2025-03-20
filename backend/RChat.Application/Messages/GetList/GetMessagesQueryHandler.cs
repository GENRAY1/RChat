using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;

namespace RChat.Application.Messages.GetList;

public class GetMessagesQueryHandler(
    IMessageRepository messageRepository
    ) : IQueryHandler<GetMessagesQuery, List<MessageDto>>
{
    public async Task<List<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        List<Message> messages =
            await messageRepository.GetListAsync(new GetMessageListParameters
            {
                ChatId = request.ChatId,
                Pagination = request.Pagination,
                Sorting = request.Sorting
            });

        return messages.Select(message => new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            CreatedAt = message.CreatedAt,
            ReplyToMessageId = message.ReplyToMessageId,
            SenderId = message.SenderId,
            Text = message.Text,
            UpdatedAt = message.UpdatedAt
        }).ToList();
    }
}