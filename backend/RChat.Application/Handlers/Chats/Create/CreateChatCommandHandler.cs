using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Application.Services.BackgroundTaskQueue;
using RChat.Application.Services.Search.Chat;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Handlers.Chats.Create;

public class CreateChatCommandHandler(
    IChatRepository chatRepository,
    IAuthContext authContext,
    IUserRepository userRepository,
    IChatSearchService chatSearchService,
    IMemberRepository memberRepository,
    IBackgroundTaskQueue backgroundTaskQueue
    ) : ICommandHandler<CreateChatCommand, CreateChatDtoResponse>
{
     public async Task<CreateChatDtoResponse> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        IChatCreationStrategy creationStrategy = request.Type switch
        {
            ChatType.Private => new CreatePrivateChatStrategy(authUser, chatRepository, userRepository, memberRepository),
            ChatType.Group => new CreateGroupChatStrategy(authUser, chatRepository, memberRepository, chatSearchService, backgroundTaskQueue),
            _ => throw new ValidationException("Invalid chat type")
        };

        CreateChatDtoResponse response = await creationStrategy.CreateChatAsync(request);

        return response;
    }
}