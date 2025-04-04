using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Chats.Create;

public class CreateChatCommandHandler(
    IChatRepository chatRepository,
    IAuthContext authContext,
    IUserRepository userRepository,
    IMemberRepository memberRepository)
    : ICommandHandler<CreateChatCommand, int>
{
     public async Task<int> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        IChatCreationStrategy creationStrategy = request.Type switch
        {
            ChatType.Private => new CreatePrivateChatAsync(authUser, chatRepository, userRepository, memberRepository),
            ChatType.Group => new CreateGroupChatAsync(authUser, chatRepository, memberRepository),
            _ => throw new ValidationException("Invalid chat type")
        };

        int chatId = await creationStrategy.CreateChatAsync(request);

        return chatId;
    }
}