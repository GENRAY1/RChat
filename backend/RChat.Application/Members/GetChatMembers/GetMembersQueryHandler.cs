using RChat.Application.Abstractions.Messaging;
using RChat.Application.Exceptions;
using RChat.Application.Members.Dtos;
using RChat.Application.Services.Authentication;
using RChat.Domain.Accounts;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;

namespace RChat.Application.Members.GetChatMembers;

public class GetMembersQueryHandler(
    IMemberRepository memberRepository,
    IChatRepository chatRepository,
    IAuthContext authContext
) : IQueryHandler<GetMembersQuery, List<MemberDto>>
{
    public async Task<List<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        if (authContext.Role == AccountRole.User.Name)
        {
            Chat? chat = await chatRepository.GetByIdAsync(request.ChatId);
            
            if (chat is null)
                throw new EntityNotFoundException(nameof(Chat), request.ChatId);

            if(chat.DeletedAt is not null)
                throw new ChatDeletedException();
            
            if (chat.IsClosed)
            {
                User authUser = await authContext.GetUserAsync();
                
                Member? member = await memberRepository.GetAsync(new GetMemberParameters
                {
                    ChatId = request.ChatId,
                    UserId = authUser.Id
                });

                if (member is null)
                    throw new UserAccessDeniedException(authUser.Id, nameof(Chat), request.ChatId);
            }
        }

        List<Member> members =
            await memberRepository.GetListAsync(new GetMemberListParameters
            {
                Pagination = request.Pagination,
                Sorting = request.Sorting,
                ChatIds = [request.ChatId]
            });
        
        return members.Select(m => new MemberDto
        {
            Id = m.Id,
            ChatId = m.ChatId,
            UserId = m.UserId,
            JoinedAt = m.JoinedAt,
            User = new User()
            {
                Id = m.User.Id,
                AccountId = m.User.AccountId,
                Firstname = m.User.Firstname,
                Lastname = m.User.Lastname,
                Username = m.User.Username,
                DateOfBirth = m.User.DateOfBirth,
                Description = m.User.Description,
                CreatedAt =  m.User.CreatedAt,
                UpdatedAt = m.User.UpdatedAt
            }
        }).ToList();
    }
}