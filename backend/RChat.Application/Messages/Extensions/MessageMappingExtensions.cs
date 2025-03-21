using RChat.Application.Messages.Dtos;
using RChat.Domain.Messages;

namespace RChat.Application.Messages.Extensions;

public static class MessageMappingExtensions
{
    public static MessageDto MappingToDto(this Message message) => new()
    {
        Id = message.Id,
        ChatId = message.ChatId,
        CreatedAt = message.CreatedAt,
        ReplyToMessageId = message.ReplyToMessageId,
        SenderId = message.SenderId,
        Text = message.Text,
        UpdatedAt = message.UpdatedAt,
        DeletedAt = message.DeletedAt
    };
}