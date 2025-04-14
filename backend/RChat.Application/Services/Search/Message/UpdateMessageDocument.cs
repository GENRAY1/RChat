namespace RChat.Application.Services.Search.Message;

public class UpdateMessageDocument
{
    public string? Text { get; init; }
    
    public DateTime? DeletedAt { get; init; }
}