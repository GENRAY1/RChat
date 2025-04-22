using RChat.Domain.Chats;

namespace RChat.Application.GlobalSearch;

public class ChatSearchDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required ChatType Type { get; init; } 
    public int MemberCount { get; init; }
}