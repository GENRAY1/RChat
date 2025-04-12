using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Application.Messages.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.GetCurrentUserChats;

public class GetCurrentUserChatsQueryHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IMessageRepository messageRepository,
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
        
        if(chats.Count == 0) return [];
        
        int[] chatIds = chats
            .Select(c => c.Id)
            .ToArray();
        
        List<Message> latestMessages = 
            await messageRepository.GetLatestMessagesByChatIdsAsync(chatIds);
        
        Dictionary<int, User> privateChatRecipientDict = 
            await GetPrivateChatRecipientDictionary(chats, authUser.Id);
        
        return chats.Select(chat =>
        {
            var latestMessage = latestMessages.FirstOrDefault(x => x.ChatId == chat.Id);
            
            return new CurrentUserChatDto
            {
                Id = chat.Id,
                DisplayName = chat.GroupChat?.Name
                              ?? privateChatRecipientDict.GetValueOrDefault(chat.Id)?.FullName
                              ?? string.Empty,
                Type = chat.Type,
                CreatorId = chat.CreatorId,
                CreatedAt = chat.CreatedAt,
                MemberCount = chat.MemberCount,
                GroupChat = chat.GroupChat?.MappingToDto(),
                LatestMessage = latestMessage?.MappingToDto(),
            };
        }).ToList();
    }

    private async Task<Dictionary<int, User>> GetPrivateChatRecipientDictionary(
        List<Chat> chats, 
        int authUserId)
    {
        Dictionary<int, User> result = [];
        
        int[] privateChatIds = chats
            .Where(chat => chat.Type == ChatType.Private)
            .Select(c => c.Id)
            .ToArray();

        if (privateChatIds.Length > 0)
        {
            List<Member> privateChatMembers = await memberRepository.GetListAsync(
                new GetMemberListParameters
                {
                    ChatIds = privateChatIds
                });
            
            foreach (var member in privateChatMembers)
            {
                if (member.UserId != authUserId)
                {
                    result.Add(member.ChatId, member.User);
                }
            }
        }
        
        return result;
    }
}