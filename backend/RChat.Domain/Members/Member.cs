using RChat.Domain.Abstractions;
using RChat.Domain.Users;

namespace RChat.Domain.Members;

public class Member : Entity
{
    public required int ChatId { get; set; }
    
    public required int UserId { get; set; }
     
    public required DateTime JoinedAt { get; set; }
    
    public User User { get; set; } = null!;
}