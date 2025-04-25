using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Handlers.Messages.SoftDelete;

public class SoftDeleteMessageCommand : ICommand<DeleteMessageDtoResponse>
{
    public required int MessageId { get; set; }
}