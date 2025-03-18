using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Users.Extensions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;

namespace RChat.Application.Members.Create;

public class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IUserRepository userRepository
) : ICommandHandler<CreateMemberCommand, int>
{
    public async Task<int> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(
            new GetUserParameters
            {
                Id = request.UserId
            });
        
        var existingMember = await memberRepository.GetAsync(
            new GetMemberParameters
            {
                ChatId = request.ChatId,
                UserId = request.UserId
            });

        if (existingMember is not null)
            throw new ValidationException("Member already exists");
        
        if(user is null)
            throw new EntityNotFoundException(nameof(User), request.UserId);

        Chat? chat = await chatRepository.GetByIdAsync(request.ChatId);

        if (chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if (chat.Type == ChatType.Private)
            throw new ValidationException("Private chat cannot have more than two members");
        
        Member member = new Member
        {
            ChatId = request.ChatId,
            UserId = request.UserId,
            JoinedAt = DateTime.UtcNow
        };
        
        int id = await memberRepository.CreateAsync(member);

        return id;
    }
}