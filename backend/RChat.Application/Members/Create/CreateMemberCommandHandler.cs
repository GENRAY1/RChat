using RChat.Application.Abstractions.Messaging;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Members.Create;

public class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
) : ICommandHandler<CreateMemberCommand, int>
{
    public async Task<int> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        User authUser = await authContext.GetUserAsync();
        
        var existingMember = await memberRepository.GetAsync(
            new GetMemberParameters
            {
                ChatId = request.ChatId,
                UserId = authUser.Id
            });

        if (existingMember is not null)
            throw new ConflictDataException(nameof(Member));
        
        Chat? chat = await chatRepository.GetByIdAsync(request.ChatId);

        if (chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if (chat.Type == ChatType.Private)
            throw new ValidationException("Private chat cannot have more than two members");
        
        if(chat.GroupChat?.IsPrivate == true)
            throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
        
        Member member = new Member
        {
            ChatId = request.ChatId,
            UserId = authUser.Id,
            JoinedAt = DateTime.UtcNow
        };
        
        int id = await memberRepository.CreateAsync(member);

        return id;
    }
}