using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Application.Chats.Extensions;

public static class ChatMappingExtensions
{
    public static ChatDto MappingToDto(this Chat chat) => new ChatDto
    {
        Id = chat.Id,
        CreatorId = chat.CreatorId,
        CreatedAt = chat.CreatedAt,
        GroupChat = chat.GroupChat?.MappingToDto(),
        DeletedAt = chat.DeletedAt,
        Type = chat.Type
    };


}