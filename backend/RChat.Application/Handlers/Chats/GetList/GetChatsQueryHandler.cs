using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Chats;
using RChat.Application.Extensions.Chats;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;

namespace RChat.Application.Handlers.Chats.GetList;

public class GetChatsQueryHandler(
    IChatRepository chatRepository
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
        
        List<Chat> chats = await chatRepository.GetListAsync(parameters);
        
        return chats
            .Select(chat => chat.MappingToDto())
            .ToList();
    }
}