using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.GetById;

public class GetChatByIdQueryHandler(
    IChatRepository chatRepository,
    IMemberRepository memberRepository,
    IAuthContext authContext
    ) : IQueryHandler<GetChatByIdQuery, ChatDto>
{
    public async Task<ChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            if(chat.DeletedAt is not null)
                throw new ChatDeletedException();

            if (chat.IsClosed)
            {
                Member? member = await memberRepository.GetAsync(new GetMemberParameters
                {
                    ChatId = request.ChatId,
                    UserId = authUser.Id
                });
            
                if (member is null)
                    throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
            }
        }
        
        return chat.MappingToDto();
    }
}