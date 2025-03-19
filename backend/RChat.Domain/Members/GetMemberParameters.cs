namespace RChat.Domain.Members;

public class GetMemberParameters
{
    public int? Id { get; init; }
    
    public int? ChatId { get; init; }
    
    public int? UserId { get; init; }
}