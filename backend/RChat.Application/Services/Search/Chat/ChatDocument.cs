using System.Text.Json.Serialization;
using RChat.Domain.Chats;

namespace RChat.Application.Services.Search.Chat;

public class ChatDocument
{
    public string Name { get; init; } = null!;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChatType Type { get; init; } 
    public bool IsPrivate { get; init; }
    public DateTime? DeletedAt { get; init; }
}