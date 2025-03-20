using RChat.Application.Abstractions.Messaging;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommand : ICommand
{
    public int MessageId { get; init; }
    
    public string Text { get; init; }
}