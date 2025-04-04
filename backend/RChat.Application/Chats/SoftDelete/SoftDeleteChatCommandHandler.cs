using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.SoftDelete;

public class SoftDeleteChatCommandHandler(
    IChatRepository chatRepository,
    IAuthContext authContext
    ) : ICommandHandler<SoftDeleteChatCommand>
{
    public async Task Handle(SoftDeleteChatCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            if(chat.CreatorId != authUser.Id)
                throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
        }
        
        chat.DeletedAt = DateTime.UtcNow;

        await chatRepository.UpdateAsync(chat);
    }
}