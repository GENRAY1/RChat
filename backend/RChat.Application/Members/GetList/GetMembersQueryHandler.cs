using RChat.Application.Abstractions.Messaging;
using RChat.Application.Members.Dtos;
using RChat.Domain.Members;

namespace RChat.Application.Members.GetList;

public class GetMembersQueryHandler(IMemberRepository memberRepository) 
    : IQueryHandler<GetMembersQuery, List<MemberDto>>
{
    public async Task<List<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        List<Member> members = 
            await memberRepository.GetListAsync(new GetMemberListParameters
            {
                Pagination = request.Pagination,
                ChatIds = request.ChatId.HasValue 
                    ? [request.ChatId.Value] 
                    : null
            });
        
        return members.Select(m => new MemberDto
        {
            ChatId = m.ChatId,
            UserId = m.UserId,
            JoinedAt = m.JoinedAt,
            User = new MemberUserDto
            {
                Id = m.User.Id,
                Username = m.User.Username,
                DateOfBirth = m.User.DateOfBirth,
                Description = m.User.Description
            }
        }).ToList();
    }
}