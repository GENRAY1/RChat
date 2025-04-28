using RChat.Application.Abstractions.Messaging;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Messages;
using RChat.Application.Exceptions;
using RChat.Application.Extensions.Messages;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Messages.GetChatMessages;

public class GetChatMessagesQueryHandler(
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
    IMemberRepository memberRepository,
    IAuthContext authContext
    ) : IQueryHandler<GetChatMessagesQuery, List<MessageDto>>
{
    public async Task<List<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        var parameters = new GetMessageListParameters
        {
            ChatId = request.ChatId,
            Pagination = request.Pagination,
            Sorting = new SortingDto<MessageSortingColumn>
            {
                Column = MessageSortingColumn.CreatedAt,
                Direction = SortingDirection.Desc
            }
        };
        
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
            
            parameters.OnlyActive = true;
        }
        
        List<Message> messages =
            await messageRepository.GetListAsync(parameters);

        return messages
            .Select(message => message.MappingToDto())
            .ToList();
    }
}