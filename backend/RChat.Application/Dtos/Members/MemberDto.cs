using RChat.Domain.Users;

namespace RChat.Application.Dtos.Members;

public class MemberDto
{
    public required int Id { get; init; }
    public required int ChatId { get; set; }
    public required int UserId { get; set; }
    public required DateTime JoinedAt { get; init; }
    public required User User { get; init; }
}