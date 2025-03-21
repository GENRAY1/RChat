using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommand : ICommand<MessageDto>
{
    public int MessageId { get; init; }
    
    public required string Text { get; init; }
}