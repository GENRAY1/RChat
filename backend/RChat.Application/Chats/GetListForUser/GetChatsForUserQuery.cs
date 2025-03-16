using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;

namespace RChat.Application.Chats.GetListForUser;

public class GetChatsForUserQuery : IQuery<List<UserChatDto>>
{
    public required int UserId { get; init; }
}