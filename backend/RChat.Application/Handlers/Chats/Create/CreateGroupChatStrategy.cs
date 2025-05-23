using RChat.Application.Exceptions;
using RChat.Application.Extensions.Chats;
using RChat.Application.Services.BackgroundTaskQueue;
using RChat.Application.Services.Search.Chat;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Handlers.Chats.Create;

public class CreateGroupChatStrategy(
    User creator,
    IChatRepository chatRepository,
    IMemberRepository memberRepository,
    IChatSearchService chatSearchService,
    IBackgroundTaskQueue backgroundTaskQueue
    ) : IChatCreationStrategy
{
    public async Task<CreateChatDtoResponse> CreateChatAsync(CreateChatCommand request)
    {
        if(request.GroupDetails is null)
            throw new ValidationException("Chat chat must have group details");
        
        DateTime now = DateTime.UtcNow;

        Chat newChat = new Chat
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
        
        newChat.Id = await chatRepository.CreateAsync(newChat);
        
        backgroundTaskQueue.Enqueue(async token =>
        {
            await chatSearchService.IndexAsync(newChat.Id, new ChatDocument
            {
                Type = request.Type,
                Name = request.GroupDetails.Name,
                IsPrivate = request.GroupDetails.IsPrivate
            }, token);
        });
        
        await memberRepository.CreateAsync(new Member
        {
            ChatId = newChat.Id,
            UserId = creator.Id,
            JoinedAt = now
        });
        
        return new CreateChatDtoResponse
        {
            Id = newChat.Id,
            Type = newChat.Type,
            CreatorId = newChat.CreatorId,
            GroupChat = newChat.GroupChat.MappingToDto(),
            CreatedAt = newChat.CreatedAt,
            MemberCount = 1
        };;
    }
}