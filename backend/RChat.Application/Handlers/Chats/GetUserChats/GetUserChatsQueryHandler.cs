using RChat.Application.Abstractions.Messaging;
using RChat.Application.Dtos.Chats;
using RChat.Application.Extensions.Chats;
using RChat.Application.Extensions.Messages;
using RChat.Application.Services.Authentication;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Chats.GetUserChats;

public class GetUserChatsQueryHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
) : IQueryHandler<GetUserChatsQuery, List<UserChatDto>>
{
    public async Task<List<UserChatDto>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        List<UserChat> chats = await chatRepository.GetUserChatsAsync(
            new GetUserChatsParameters
            {
                UserId = authUser.Id,
                SecondUserId = request.SecondUserId,
                ChatIds = request.ChatIds,
                Pagination = request.Pagination,
                Type = request.Type
            });

        ILookup<int, PrivateChatMemberDto>? privateChatMembers = await GetPrivateChatMembers(chats);

        return chats.Select(c => new UserChatDto
        {
            Id = c.Id,
            Type = c.Type,
            CreatorId = c.CreatorId,
            CreatedAt = c.CreatedAt,
            MemberCount = c.MemberCount,
            GroupChat = c.GroupChat?.MappingToDto(),
            PrivateChatMembers = c.Type == ChatType.Private 
                ? privateChatMembers?[c.Id].ToList()
                : null,
            LatestMessage = c.LatestMessage?.MappingToDto(),
        }).ToList();
    }

    private async Task<ILookup<int, PrivateChatMemberDto>?> GetPrivateChatMembers(List<UserChat> chats)
    {
        var privateChatIds = chats
            .Where(c => c.Type == ChatType.Private)
            .Select(c => c.Id)
            .ToArray();

        if (!privateChatIds.Any()) return null;

        var members = await memberRepository.GetListAsync(
            new GetMemberListParameters
            {
                ChatIds = privateChatIds
            });

        return members.ToLookup(m => m.ChatId, m => new PrivateChatMemberDto
        {
            MemberId = m.Id,
            UserId = m.UserId,
            Firstname = m.User.Firstname,
            Lastname = m.User.Lastname,
            Username = m.User.Username
        });
    }
}