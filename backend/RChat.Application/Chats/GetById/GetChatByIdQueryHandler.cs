using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;

namespace RChat.Application.Chats.GetById;

public class GetChatByIdQueryHandler(IChatRepository chatRepository)
    : IQueryHandler<GetChatByIdQuery, ChatDto>
{
    public async Task<ChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        Chat? chat = 
            await chatRepository.GetByIdAsync(request.ChatId); 
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        return chat.MappingToDto();
    }
}