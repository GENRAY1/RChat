using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Application.Messages.Dtos;
using RChat.Application.Messages.Extensions;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Common;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Messages.GetChatMessages;

public class GetChatMessagesQueryHandler(
    IMessageRepository messageRepository,
    IMemberRepository memberRepository,
    IAuthContext authContext
    ) : IQueryHandler<GetChatMessagesQuery, List<MessageDto>>
{
    public async Task<List<MessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
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
            
            Member? member = await memberRepository.GetAsync(new GetMemberParameters
            {
                ChatId = request.ChatId,
                UserId = authUser.Id
            });
            
            if (member is null)
                throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
            
            parameters.OnlyActive = true;
        }
        
        List<Message> messages =
            await messageRepository.GetListAsync(parameters);

        return messages
            .Select(message => message.MappingToDto())
            .ToList();
    }
}