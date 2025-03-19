using RChat.Application.Members.Dtos;

namespace RChat.Web.Controllers.Chats.GetChatMembers;

public class GetChatMembersResponse
{
    public required List<MemberDto> Members { get; init; }
}