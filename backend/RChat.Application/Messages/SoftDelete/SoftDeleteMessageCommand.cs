using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Messages.SoftDelete;

public class SoftDeleteMessageCommand : ICommand<DeleteMessageDtoResponse>
{
    public required int MessageId { get; set; }
}