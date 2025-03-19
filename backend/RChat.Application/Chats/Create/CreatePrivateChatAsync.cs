using RChat.Application.Exceptions;
using RChat.Application.Users.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Chats.Create;

public class CreatePrivateChatAsync(
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IMemberRepository memberRepository)
    : IChatCreationStrategy
{
    public async Task<int> CreateChatAsync(CreateChatCommand request)
    {
        if (request.RecipientId is null)
            throw new ValidationException("Private chat must have a recipient");

        User recipient =
            await userRepository.GetByIdOrThrowAsync(request.RecipientId.Value);

        User creator =
            await userRepository.GetByIdOrThrowAsync(request.CreatorId);
        
        bool hasSharedPrivateChat = await HasSharedPrivateChat(creator.Id, recipient.Id);

        if(hasSharedPrivateChat)
            throw new ValidationException("Private chat already exists");
        
        var now = DateTime.UtcNow;

        var chatId = await chatRepository.CreateAsync(new Chat
        {
            Type = ChatType.Private,
            CreatorId = creator.Id,
            CreatedAt = now
        });

        await AddChatMembers(chatId, now, creator.Id, recipient.Id);

        return chatId;
    }

    private async Task<bool> HasSharedPrivateChat(int firstUserId, int secondUserId)
    {
        List<Member> members = await memberRepository.GetListAsync(new GetMemberListParameters
        {
            UserIds = [firstUserId, secondUserId]
        });

        IEnumerable<int> firstUserChatIds = members
            .Where(m => m.UserId == firstUserId)
            .Select(m => m.ChatId);
        
        IEnumerable<int> secondUserChatIds = members
            .Where(m => m.UserId == secondUserId)
            .Select(m => m.ChatId);

        int[] sharedChatIds = firstUserChatIds
            .Intersect(secondUserChatIds)
            .ToArray();
        
        if(!sharedChatIds.Any())
            return false;

        List<Chat> sharedPrivateChats = await chatRepository.GetListAsync(
            new GetChatListParameters
            {
                ChatIds = sharedChatIds,
                OnlyActive = true,
                Type = ChatType.Private
            });
        
        return sharedPrivateChats.Any();
    }
    
    private async Task AddChatMembers(
        int chatId,
        DateTime now,
        params int[] userIds)
    {
        var members = userIds.Select(userId =>
            new Member
            {
                ChatId = chatId,
                UserId = userId,
                JoinedAt = now
            }).ToList();

        await memberRepository.CreateAsync(members);
    }
}