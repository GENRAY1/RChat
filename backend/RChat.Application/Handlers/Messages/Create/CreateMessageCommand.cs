using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Messages;

namespace RChat.Application.Handlers.Messages.Create;

public class CreateMessageCommand : ICommand<MessageDto>
{
    public required int ChatId { get; init; }
    
    public required string Text { get; init; }
    
    public int? ReplyToMessageId { get; init; }
}