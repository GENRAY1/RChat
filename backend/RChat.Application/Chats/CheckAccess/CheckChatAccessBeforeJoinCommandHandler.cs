using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;

namespace RChat.Application.Chats.CheckAccess;

public class CheckChatAccessBeforeJoinCommandHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IUserContext userContext
) : ICommandHandler<CheckChatAccessBeforeJoinCommand>
{
    public async Task Handle(CheckChatAccessBeforeJoinCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);

        if(chat.DeletedAt is not null)
            throw new ValidationException("Chat is deleted");
        
        bool isClosedChat = chat.Type == ChatType.Private || chat.GroupChat?.IsPrivate == true;
        
        if (isClosedChat)
        {
            Member? member = await memberRepository.GetAsync(new GetMemberParameters
            {
                ChatId = request.ChatId,
                UserId = userContext.UserId
            });

            if (member is null)
                throw new UserAccessDeniedException(userContext.UserId, "chat", request.ChatId);
        }
    }
}