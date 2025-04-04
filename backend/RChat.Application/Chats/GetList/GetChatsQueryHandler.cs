using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.GetList;

public class GetChatsQueryHandler(
    IChatRepository chatRepository,
    IAuthContext authContext
    ) : IQueryHandler<GetChatsQuery, List<ChatDto>>
{
    public async Task<List<ChatDto>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        GetChatListParameters parameters = new GetChatListParameters
        {
            Type = request.Type,
            Sorting = request.Sorting,
            Pagination = request.Pagination
        };

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            parameters.OnlyActive = true;
            parameters.OnlyAccessibleByUserId = authUser.Id;
        }
        
        List<Chat> chats = await chatRepository.GetListAsync(parameters);
        
        return chats
            .Select(chat => chat.MappingToDto())
            .ToList();
    }
}