using RChat.Application.Abstractions;
using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.Search.Chat;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.Update;

public class UpdateChatCommandHandler(
    IChatRepository chatRepository,
    IChatSearchService chatSearchService,
    IBackgroundTaskQueue backgroundTaskQueue,
    IAuthContext authContext
    ) : ICommandHandler<UpdateChatCommand>
{
    public async Task Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = await chatRepository.GetByIdAsync(request.ChatId);
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if(chat.Type == ChatType.Private)
            throw new ValidationException("Private chat cannot be updated");

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            if(chat.CreatorId != authUser.Id)
                throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
        }
        
        if (chat.Type == ChatType.Group)
        {
            if(request.GroupDetails is null)
                throw new ValidationException("Chat chat must have group details");
            
            if(chat.GroupChat is null)
                throw new ValidationException("Chat chat details are missing");
            
            bool needUpdateGroupDocument = 
                chat.GroupChat.Name != request.GroupDetails.Name ||
                chat.GroupChat.IsPrivate != request.GroupDetails.IsPrivate;
            
            chat.GroupChat.Name = request.GroupDetails.Name;
            chat.GroupChat.Description = request.GroupDetails.Description;
            chat.GroupChat.IsPrivate = request.GroupDetails.IsPrivate;
            
            await chatRepository.UpdateAsync(chat);

            if (needUpdateGroupDocument)
            {
                backgroundTaskQueue.Enqueue(async token =>
                {
                    await chatSearchService.UpdateAsync(
                        chat.Id,
                        new UpdateChatDocument
                        {
                            Name = chat.GroupChat.Name,
                            IsPrivate = chat.GroupChat.IsPrivate
                        }, token);
                });
            }
        }
    }
}