using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.Extensions;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Chats;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommandHandler(
    IMessageRepository messageRepository,
    IAuthContext authContext
    ) : ICommandHandler<UpdateMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
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

        return message.MappingToDto();
    }
}