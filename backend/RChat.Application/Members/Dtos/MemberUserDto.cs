namespace RChat.Application.Members.Dtos;

public class MemberUserDto
{
    public int Id { get; init; }
    
    public required string Username { get; init; }
    
    public string? Description { get; init; }
    
    public DateTime? DateOfBirth { get; init; }
}