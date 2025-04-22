using RChat.Application.Abstractions;
using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.Search.Message;
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
    IAuthContext authContext,
    IMessageSearchService messageSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
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

        if (chat.DeletedAt is not null)
            throw new ValidationException("Cannot create message to deleted chat");

        Message message = new Message
        {
            ChatId = request.ChatId,
            Text = request.Text,
            SenderId = authUser.Id,
            ReplyToMessageId = request.ReplyToMessageId,
            CreatedAt = DateTime.UtcNow
        };

        message.Id = await messageRepository.CreateAsync(message);

        backgroundTaskQueue.Enqueue(async token =>
        {
            await messageSearchService.IndexAsync(
                message.Id,
                new MessageDocument
                {
                    Text = message.Text,
                    ChatId = message.ChatId,
                    SenderId = message.SenderId
                }, 
                token);
        });

        return new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            CreatedAt = message.CreatedAt,
            ReplyToMessageId = message.ReplyToMessageId,
            Text = message.Text,
            Sender = new MessageSenderDto
            {
                UserId = authUser.Id,
                Lastname = authUser.Lastname,
                Firstname = authUser.Firstname,
                Username = authUser.Username
            }
        };
    }
}