using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Users.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace RChat.Application.Messages.Create;

public class CreateMessageCommandHandler(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IMemberRepository memberRepository,
    IUserContext userContext
) : ICommandHandler<CreateMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        Member? member = await memberRepository.GetAsync(new GetMemberParameters
        {
            ChatId = request.ChatId,
            UserId = userContext.UserId
        });
        
        if (member is null)
            throw new UserAccessDeniedException(userContext.UserId, "chat", request.ChatId);
        
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId);

        if (chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if(chat.DeletedAt is not null)
            throw new ValidationException("Cannot create message to deleted chat");
        
        Message message = new Message
        {
            ChatId = request.ChatId,
            Text = request.Text,
            SenderId = userContext.UserId,
            ReplyToMessageId = request.ReplyToMessageId,
            CreatedAt = DateTime.UtcNow
        };

        int messageId = await messageRepository.CreateAsync(message);

        return new MessageDto
        {
            Id = messageId,
            ChatId = message.ChatId,
            CreatedAt = message.CreatedAt,
            ReplyToMessageId = message.ReplyToMessageId,
            SenderId = message.SenderId,
            Text = message.Text
        };
    }
}