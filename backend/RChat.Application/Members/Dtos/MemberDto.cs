using RChat.Domain.Users;

namespace RChat.Application.Members.Dtos;

public class MemberDto
{
    public required int ChatId { get; set; }
    
    public required int UserId { get; set; }
     
    public required DateTime JoinedAt { get; init; }
    
    public required MemberUserDto User { get; init; }
}