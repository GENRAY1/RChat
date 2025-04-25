using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.BackgroundTaskQueue;
using RChat.Application.Services.Search.Message;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Messages.SoftDelete;

public class SoftDeleteMessageCommandHandler(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IAuthContext authContext,
    IMessageSearchService messageSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
    ) : ICommandHandler<SoftDeleteMessageCommand, DeleteMessageDtoResponse>
{
    public async Task<DeleteMessageDtoResponse> Handle(SoftDeleteMessageCommand request, CancellationToken cancellationToken)
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

        await messageSearchService.UpdateAsync(
            message.Id,
            new UpdateMessageDocument { DeletedAt = message.DeletedAt },
            cancellationToken);
        
        backgroundTaskQueue.Enqueue(async token =>
        {
            await messageSearchService.UpdateAsync(
                message.Id,
                new UpdateMessageDocument { DeletedAt = message.DeletedAt },
                token);
        });
        
        return new DeleteMessageDtoResponse
        {
            MessageId = message.Id,
            DeletedAt = message.DeletedAt!.Value,
            ChatId = message.ChatId
        };
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