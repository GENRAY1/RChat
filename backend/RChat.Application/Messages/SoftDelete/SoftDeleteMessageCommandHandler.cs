using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.Extensions;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Messages.SoftDelete;

public class SoftDeleteMessageCommandHandler(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
    ) : ICommandHandler<SoftDeleteMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SoftDeleteMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message = 
            await messageRepository.GetByIdAsync(request.MessageId);

        if (message is null)
            throw new EntityNotFoundException(nameof(Message), request.MessageId);

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            bool canUserDeleteMessage = await CanUserDeleteMessage(message, authUser.Id);
            
            if (!canUserDeleteMessage)
                throw new UserAccessDeniedException(authUser.Id, nameof(Message), request.MessageId);
        }
        
        message.DeletedAt = DateTime.UtcNow;
        
        await messageRepository.UpdateAsync(message);
        
        return message.MappingToDto();
    }

    private async Task<bool> CanUserDeleteMessage(Message message, int userId)
    {
        if (message.SenderId == userId)
            return true;
        
        Chat? chat = 
            await chatRepository.GetByIdAsync(message.ChatId);
        
        if(chat?.CreatorId == userId)
            return true;
        
        return false;
    } 
}