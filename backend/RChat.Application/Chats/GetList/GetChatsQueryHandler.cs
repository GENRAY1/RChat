using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;

namespace RChat.Application.Chats.GetList;

public class GetChatsQueryHandler(IChatRepository chatRepository) 
    : IQueryHandler<GetChatsQuery, List<ChatDto>>
{
    public async Task<List<ChatDto>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        List<Chat> chats = await chatRepository.GetListAsync(
            new GetChatListParameters
            {
                Pagination = request.Pagination
            });
        
        return chats
            .Select(chat => chat.MappingToDto())
            .ToList();
    }
}