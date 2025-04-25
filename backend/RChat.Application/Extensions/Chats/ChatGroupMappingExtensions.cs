using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Extensions.Chats;

public static class ChatGroupMappingExtensions
{
    public static ChatGroupDto MappingToDto(this ChatGroup chat) => new ChatGroupDto
    {
        Name = chat.Name,
        Description = chat.Description,
        IsPrivate = chat.IsPrivate,
    };
}