using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.BackgroundTaskQueue;
using RChat.Application.Services.Search.Chat;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Chats.SoftDelete;

public class SoftDeleteChatCommandHandler(
    IChatRepository chatRepository,
    IAuthContext authContext,
    IChatSearchService chatSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
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
        
        if (chat.Type == ChatType.Group)
        {
            backgroundTaskQueue.Enqueue(async token =>
            {
                await chatSearchService.UpdateAsync(
                    chat.Id,
                    new UpdateChatDocument { DeletedAt = chat.DeletedAt },
                    token);
            });
        }

        await chatRepository.UpdateAsync(chat);
    }
}