using RChat.Application.Exceptions;
using RChat.Application.Users.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Chats.Create;

public class CreateGroupChatAsync(
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IMemberRepository memberRepository) 
    : IChatCreationStrategy
{
    public async Task<int> CreateChatAsync(CreateChatCommand request)
    {
        if(request.GroupDetails is null)
            throw new ValidationException("Group chat must have group details");
        
        DateTime now = DateTime.UtcNow;
        
        User creator = await userRepository.GetByIdOrThrowAsync(request.CreatorId);

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