namespace RChat.Application.Dtos.Chats;

public class PrivateChatMembersDto
{
    public required PrivateChatMemberDto FirstMember { get; set; }
    public required PrivateChatMemberDto SecondMember { get; set; }
}