namespace RChat.Application.Messages.Dtos;

public class MessageSenderDto
{
    public required int UserId { get; init; }
    
    public required string Firstname { get; init; }
    
    public string? Lastname { get; init; } 
    
    public string? Username { get; init; }
}