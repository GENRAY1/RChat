using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;

namespace RChat.Application.Messages.SoftDelete;

public class SoftDeleteMessageCommandHandler(
    IMessageRepository messageRepository
    ) : ICommandHandler<SoftDeleteMessageCommand>
{
    public async Task Handle(SoftDeleteMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message = 
            await messageRepository.GetByIdAsync(request.MessageId);

        if (message is null)
            throw new EntityNotFoundException(nameof(Message), request.MessageId);
        
        message.DeletedAt = DateTime.UtcNow;
        
        await messageRepository.UpdateAsync(message);
    }
}