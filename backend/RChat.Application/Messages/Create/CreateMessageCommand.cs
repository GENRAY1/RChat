using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Messages.Create;

public class CreateMessageCommand : ICommand<int>
{
    public required int ChatId { get; init; }
    
    public required string Text { get; init; }
    
    public int? ReplyToMessageId { get; init; }
    
    public required int SenderId { get; set; }
}