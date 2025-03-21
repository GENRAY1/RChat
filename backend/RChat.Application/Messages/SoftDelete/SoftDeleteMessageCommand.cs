using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;

namespace RChat.Application.Messages.SoftDelete;

public class SoftDeleteMessageCommand : ICommand<MessageDto>
{
    public required int MessageId { get; set; }
}