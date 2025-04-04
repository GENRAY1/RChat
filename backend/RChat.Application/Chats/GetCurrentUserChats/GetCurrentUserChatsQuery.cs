using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;

namespace RChat.Application.Chats.GetCurrentUserChats;

public class GetCurrentUserChatsQuery : IQuery<List<CurrentUserChatDto>>{}