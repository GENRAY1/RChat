using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.GetCurrentUserChats;

public class GetCurrentUserChatsQueryHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
) : IQueryHandler<GetCurrentUserChatsQuery, List<CurrentUserChatDto>>
{
    public async Task<List<CurrentUserChatDto>> Handle(GetCurrentUserChatsQuery request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        List<Chat> chats = await chatRepository.GetListAsync(
            new GetChatListParameters
            {
                UserIds = [authUser.Id],
                OnlyActive = true
            });
        
        int[] privateChatIds = chats
            .Where(chat => chat.Type == ChatType.Private)
            .Select(c => c.Id)
            .ToArray();

        Dictionary<int, User> privateChatRecipientDict = [];

        if (privateChatIds.Length > 0)
        {
            List<Member> privateChatMembers = await memberRepository.GetListAsync(
                new GetMemberListParameters
                {
                    ChatIds = privateChatIds
                });
            
            foreach (var member in privateChatMembers)
            {
                if (member.UserId != authUser.Id)
                {
                    privateChatRecipientDict.Add(member.ChatId, member.User);
                }
            }
        }
        return chats.Select(chat => new CurrentUserChatDto
        {
            Id = chat.Id,
            DisplayName = chat.GroupChat?.Name 
                ?? privateChatRecipientDict.GetValueOrDefault(chat.Id)?.FullName 
                ?? string.Empty,
            Type = chat.Type,
            CreatorId = chat.CreatorId,
            CreatedAt = chat.CreatedAt,
            GroupChat = chat.GroupChat?.MappingToDto()
        }).ToList();
    }
}