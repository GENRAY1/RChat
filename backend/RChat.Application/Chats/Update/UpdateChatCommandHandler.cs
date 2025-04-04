using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.Update;

public class UpdateChatCommandHandler(
    IChatRepository chatRepository,
    IAuthContext authContext)
    : ICommandHandler<UpdateChatCommand>
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
                throw new ValidationException("Group chat must have group details");
            
            if(chat.GroupChat is null)
                throw new ValidationException("Group chat details are missing");
            
            chat.GroupChat.Name = request.GroupDetails.Name;
            chat.GroupChat.Description = request.GroupDetails.Description;
            chat.GroupChat.IsPrivate = request.GroupDetails.IsPrivate;
            
            await chatRepository.UpdateAsync(chat);
        }
    }
}