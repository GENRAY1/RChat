using RChat.Application.Abstractions.Messaging;
using RChat.Application.Extensions.Messages;

namespace RChat.Application.Handlers.Messages.Update;

public class UpdateMessageCommand : ICommand<UpdateMessageDtoResponse>
{
    public int MessageId { get; init; }
    
    public required string Text { get; init; }
}