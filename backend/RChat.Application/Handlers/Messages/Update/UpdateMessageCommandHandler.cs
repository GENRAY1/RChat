using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Extensions.Messages;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.BackgroundTaskQueue;
using RChat.Application.Services.Search.Message;
using RChat.Domain.Accounts;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Messages.Update;

public class UpdateMessageCommandHandler(
    IMessageRepository messageRepository,
    IMessageSearchService messageSearchService,
    IBackgroundTaskQueue backgroundTaskQueue,
    IAuthContext authContext
    ) : ICommandHandler<UpdateMessageCommand, UpdateMessageDtoResponse>
{
    public async Task<UpdateMessageDtoResponse> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message =
            await messageRepository.GetByIdAsync(request.MessageId);
        
        if (message is null)
            throw new EntityNotFoundException(nameof(Message), request.MessageId);

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            if(message.SenderId != authUser.Id)
                throw new UserAccessDeniedException(authUser.Id, nameof(Message), message.Id);
        }
        
        message.Text = request.Text;
        message.UpdatedAt = DateTime.UtcNow;
        
        await messageRepository.UpdateAsync(message);

        await messageSearchService.UpdateAsync(
            message.Id,
            new UpdateMessageDocument { Text = message.Text }, 
            cancellationToken);
        
        
        backgroundTaskQueue.Enqueue(async token =>
        {
            await messageSearchService.UpdateAsync(
                message.Id,
                new UpdateMessageDocument { Text = message.Text }, 
                token);
        });
        
        return new UpdateMessageDtoResponse
        {
            MessageId = message.Id,
            ChatId = message.ChatId,
            Text = message.Text,
            UpdatedAt = message.UpdatedAt!.Value
        };
    }
}