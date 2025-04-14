using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.CheckAccess;

public class CheckChatAccessBeforeJoinCommandHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
) : ICommandHandler<CheckChatAccessBeforeJoinCommand>
{
    public async Task Handle(CheckChatAccessBeforeJoinCommand request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);

        if(chat.DeletedAt is not null)
            throw new ChatDeletedException();
        
        if (chat.IsClosed)
        {
            User authUser = await authContext.GetUserAsync(); 
            
            Member? member = await memberRepository.GetAsync(new GetMemberParameters
            {
                ChatId = request.ChatId,
                UserId = authUser.Id
            });

            if (member is null)
                throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
        }
    }
}