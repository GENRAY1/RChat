namespace RChat.Application.Services.Search.Chat;

public class UpdateChatDocument
{
    public string? Name { get; init; }
    public bool? IsPrivate { get; init; }
    public DateTime? DeletedAt { get; init; }
}