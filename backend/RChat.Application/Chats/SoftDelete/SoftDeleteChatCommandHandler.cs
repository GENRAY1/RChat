using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;

namespace RChat.Application.Chats.SoftDelete;

public class SoftDeleteChatCommandHandler(IChatRepository chatRepository)
    : ICommandHandler<SoftDeleteChatCommand>
{
    public async Task Handle(SoftDeleteChatCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        chat.DeletedAt = DateTime.UtcNow;

        await chatRepository.UpdateAsync(chat);
    }
}