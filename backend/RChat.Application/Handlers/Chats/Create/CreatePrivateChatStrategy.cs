using RChat.Application.Dtos.Chats;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Handlers.Chats.Create;

public class CreatePrivateChatStrategy(
    User creator,
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IMemberRepository memberRepository)
    : IChatCreationStrategy
{
    public async Task<CreateChatDtoResponse> CreateChatAsync(CreateChatCommand request)
    {
        if (request.RecipientId is null)
            throw new ValidationException("Private chat must have a recipient");
        
        User? recipient = await userRepository.GetAsync(
            new GetUserParameters
            {
                Id = request.RecipientId.Value
            });
        
        if (recipient is null)
            throw new EntityNotFoundException(nameof(User), request.RecipientId.Value);
        
        bool hasSharedPrivateChat = await HasSharedPrivateChat(creator.Id, recipient.Id);

        if(hasSharedPrivateChat)
            throw new ValidationException("Private chat already exists");
        
        var now = DateTime.UtcNow;

        Chat newChat = new Chat
        {
            Type = ChatType.Private,
            CreatorId = creator.Id,
            CreatedAt = now
        };
        
        int chatId = await chatRepository.CreateAsync(newChat);
        
        List<PrivateChatMemberDto> privateChatMembers = 
            await AddPrivateChatMembers(chatId, now, recipient, creator);

        return new CreateChatDtoResponse
        {
            Id = chatId,
            Type = newChat.Type,
            CreatorId = newChat.CreatorId,
            CreatedAt = newChat.CreatedAt,
            MemberCount = privateChatMembers.Count,
            PrivateChatMembers = privateChatMembers
        };
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
    
    private async Task<List<PrivateChatMemberDto>> AddPrivateChatMembers(
        int chatId,
        DateTime now,
        params User[] users)
    {
        List<PrivateChatMemberDto> result = []; 
        
        foreach (var u in users)
        {
            Member member = new Member
            {
                ChatId = chatId,
                UserId = u.Id,
                JoinedAt = now
            };

            int memberId = await memberRepository.CreateAsync(member);
            
            result.Add(new PrivateChatMemberDto
            {
                MemberId = memberId,
                UserId = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Username = u.Username,
            });
        }

        return result;
    }
}