using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Chats.Create;

public class CreateChatCommandHandler(
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IMemberRepository memberRepository)
    : ICommandHandler<CreateChatCommand, int>
{
     public async Task<int> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        IChatCreationStrategy creationStrategy = request.Type switch
        {
            ChatType.Private => new CreatePrivateChatAsync(chatRepository, userRepository, memberRepository),
            ChatType.Group => new CreateGroupChatAsync(chatRepository, userRepository, memberRepository),
            _ => throw new ValidationException("Invalid chat type")
        };

        int chatId = await creationStrategy.CreateChatAsync(request);

        return chatId;
    }
}