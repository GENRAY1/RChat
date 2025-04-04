using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Messages.Create;

public class CreateMessageCommandHandler(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IMemberRepository memberRepository,
    IAuthContext authContext
) : ICommandHandler<CreateMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        Member? member = await memberRepository.GetAsync(new GetMemberParameters
        {
            ChatId = request.ChatId,
            UserId = authUser.Id
        });
        
        if (member is null)
            throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
        
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
            SenderId = authUser.Id,
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