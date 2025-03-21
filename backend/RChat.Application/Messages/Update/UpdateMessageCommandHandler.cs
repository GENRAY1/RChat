using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.Extensions;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;

namespace RChat.Application.Messages.Update;

public class UpdateMessageCommandHandler(
    IMessageRepository messageRepository,
    IMemberRepository memberRepository,
    IUserContext userContext
    ) : ICommandHandler<UpdateMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        Message? message =
            await messageRepository.GetByIdAsync(request.MessageId);
        
        if (message is null)
            throw new EntityNotFoundException(nameof(Message), request.MessageId);
        
        Member? member = await memberRepository.GetAsync(new GetMemberParameters
        {
            ChatId = message.ChatId,
            UserId = userContext.UserId
        });
        
        if (member is null)
            throw new UserAccessDeniedException(userContext.UserId, "chat", message.ChatId);
        
        message.Text = request.Text;
        message.UpdatedAt = DateTime.UtcNow;
        
        await messageRepository.UpdateAsync(message);

        return message.MappingToDto();
    }
}