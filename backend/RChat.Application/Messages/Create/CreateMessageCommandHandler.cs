using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Users.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace RChat.Application.Messages.Create;

public class CreateMessageCommandHandler(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IUserRepository userRepository
) : ICommandHandler<CreateMessageCommand, int>
{
    public async Task<int> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId);

        if (chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if(chat.DeletedAt is not null)
            throw new ValidationException("Cannot create message to deleted chat");
        
        User user = 
            await userRepository.GetByIdOrThrowAsync(request.SenderId);
        
        Message message = new Message
        {
            ChatId = request.ChatId,
            Text = request.Text,
            SenderId = user.Id,
            ReplyToMessageId = request.ReplyToMessageId,
            CreatedAt = DateTime.UtcNow
        };
        
        return await messageRepository.CreateAsync(message);
    }
}