using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;

namespace RChat.Application.Extensions.Chats;

public static class ChatMappingExtensions
{
    public static ChatDto MappingToDto(this Chat chat) => new ChatDto
    {
        Id = chat.Id,
        CreatorId = chat.CreatorId,
        CreatedAt = chat.CreatedAt,
        GroupChat = chat.GroupChat?.MappingToDto(),
        DeletedAt = chat.DeletedAt,
        MemberCount = chat.MemberCount,
        Type = chat.Type
    };
}