using RChat.Application.Abstractions.Messaging;
using RChat.Application.Chats.Dtos;
using RChat.Application.Chats.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.GetListForUser;

public class GetChatsForUserQueryHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository
) : IQueryHandler<GetChatsForUserQuery, List<UserChatDto>>
{
    public async Task<List<UserChatDto>> Handle(GetChatsForUserQuery request, CancellationToken cancellationToken)
    {
        List<Chat> chats = await chatRepository.GetListAsync(
            new GetChatListParameters
            {
                UserIds = [request.UserId],
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
                if (member.UserId != request.UserId)
                {
                    privateChatRecipientDict.Add(member.ChatId, member.User);
                }
            }
        }
        return chats.Select(chat => new UserChatDto
        {
            Id = chat.Id,
            DisplayName = chat.GroupChat?.Name 
                ?? privateChatRecipientDict.GetValueOrDefault(chat.Id)?.Username 
                ?? string.Empty,
            Type = chat.Type,
            CreatorId = chat.CreatorId,
            CreatedAt = chat.CreatedAt,
            GroupChat = chat.GroupChat?.MappingToDto()
        }).ToList();
    }
}