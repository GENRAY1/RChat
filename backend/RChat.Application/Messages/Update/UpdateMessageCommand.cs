using RChat.Application.Abstractions.Messaging;
using RChat.Application.Messages.Dtos;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommand : ICommand<UpdateMessageDtoResponse>
{
    public int MessageId { get; init; }
    
    public required string Text { get; init; }
}