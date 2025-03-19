using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Application.Chats.Extensions;

public static class ChatGroupMappingExtensions
{
    public static ChatGroupDto MappingToDto(this ChatGroup chat) => new ChatGroupDto
    {
        Name = chat.Name,
        Description = chat.Description,
        IsPrivate = chat.IsPrivate,
    };
}