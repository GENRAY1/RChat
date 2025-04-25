namespace RChat.Application.Dtos.Chats;

public class PrivateChatMemberDto
{
    public required int MemberId { get; set; }
    public required int UserId { get; set; }
    public required string Firstname { get; init; }
    public required string? Lastname { get; init; }
    public required string? Username { get; init; }
}