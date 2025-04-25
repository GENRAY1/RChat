using RChat.Application.Dtos.Messages;
using RChat.Domain.Messages;

namespace RChat.Application.Extensions.Messages;

public static class MessageMappingExtensions
{
    public static MessageDto MappingToDto(this Message message) => new()
    {
        Id = message.Id,
        ChatId = message.ChatId,
        CreatedAt = message.CreatedAt,
        ReplyToMessageId = message.ReplyToMessageId,
        Text = message.Text,
        UpdatedAt = message.UpdatedAt,
        DeletedAt = message.DeletedAt,
        Sender = new MessageSenderDto
        {
            UserId = message.Sender.Id,
            Firstname = message.Sender.Firstname,
            Lastname = message.Sender.Lastname,
            Username = message.Sender.Username,
        }
    };
}