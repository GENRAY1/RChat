using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommandHandler(
    IMessageRepository messageRepository
    ) : ICommandHandler<UpdateMessageCommand>
{
    public async Task Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message =
            await messageRepository.GetByIdAsync(request.MessageId);
        
        if (message is null)
            throw new EntityNotFoundException(nameof(Message), request.MessageId);
        
        message.Text = request.Text;
        message.UpdatedAt = DateTime.UtcNow;
        
        await messageRepository.UpdateAsync(message);
    }
}