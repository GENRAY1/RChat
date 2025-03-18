using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;

namespace RChat.Application.Members.Delete;

public class DeleteChatMemberCommandHandler( 
    IChatRepository chatRepository,
    IMemberRepository memberRepository
    ) : ICommandHandler<DeleteChatMemberCommand>
{
    public async Task Handle(DeleteChatMemberCommand request, CancellationToken cancellationToken)
    {
        Member? member = await memberRepository.GetAsync(new GetMemberParameters
        { 
            ChatId = request.ChatId,
            UserId = request.UserId
        });

        if (member is null)
            throw new EntityNotFoundException(nameof(Member));
        
        Chat? chat = await chatRepository.GetByIdAsync(request.ChatId);
        
        if(chat is null)
            throw new EntityNotFoundException(nameof(Chat), request.ChatId);
        
        if(chat.Type == ChatType.Private)
            throw new ValidationException("Members cannot be removed from a private chat");
        
        await memberRepository.DeleteAsync(member.Id);
    }
}