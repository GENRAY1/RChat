using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Members.Delete;

public class DeleteMemberCommandHandler( 
    IChatRepository chatRepository,
    IMemberRepository memberRepository,
    IAuthContext authContext
    ) : ICommandHandler<DeleteMemberCommand>
{
    public async Task Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        Member? member = await memberRepository.GetAsync(new GetMemberParameters
        { 
            Id = request.MemberId
        });

        if (member is null)
            throw new EntityNotFoundException(nameof(Member));
        
        Chat? chat = await chatRepository.GetByIdAsync(member.ChatId);
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), member.ChatId);
        
        if(chat.Type == ChatType.Private)
            throw new ValidationException("Members cannot be removed from a private chat");

        if (authContext.Role == AccountRole.User.Name)
        {
            User authUser = await authContext.GetUserAsync();
            
            bool canUserDeleteMember = await CanUserDeleteMember(member, authUser.Id);
            
            if (!canUserDeleteMember)
                throw new UserAccessDeniedException(authUser.Id, nameof(Member), member.Id);
        }
        
        await memberRepository.DeleteAsync(member.Id);
    }
    
    private async Task<bool> CanUserDeleteMember(Member member, int userId)
    {
        if (member.UserId == userId)
            return true;
        
        Chat? chat = 
            await chatRepository.GetByIdAsync(member.ChatId);
        
        if(chat?.CreatorId == userId)
            return true;
        
        return false;
    }
}