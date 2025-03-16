using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Domain.Common;

namespace RChat.Application.Chats.GetList;

public class GetChatsQuery : IQuery<List<ChatDto>>
{
    public PaginationDto? Pagination { get; init; }
    
}