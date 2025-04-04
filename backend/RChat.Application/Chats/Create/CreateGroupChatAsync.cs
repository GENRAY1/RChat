using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Chats.Create;

public class CreateGroupChatAsync(
    User creator,
    IChatRepository chatRepository,
    IMemberRepository memberRepository) 
    : IChatCreationStrategy
{
    public async Task<int> CreateChatAsync(CreateChatCommand request)
    {
        if(request.GroupDetails is null)
            throw new ValidationException("Group chat must have group details");
        
        DateTime now = DateTime.UtcNow;

        Chat chat = new Chat
        {
            CreatedAt = now,
            CreatorId = creator.Id,
            Type = ChatType.Group,
            GroupChat = new ChatGroup
            {
                Description = request.GroupDetails.Description,
                IsPrivate = request.GroupDetails.IsPrivate,
                Name = request.GroupDetails.Name
            }
        };
        
        int chatId = await chatRepository.CreateAsync(chat);
        
        await memberRepository.CreateAsync(new Member
        {
            ChatId = chatId,
            UserId = creator.Id,
            JoinedAt = now
        });
        
        return chatId;
    }
}