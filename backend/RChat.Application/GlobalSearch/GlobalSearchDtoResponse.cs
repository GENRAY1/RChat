namespace RChat.Application.GlobalSearch;

public class GlobalSearchDtoResponse
{
    public required List<ChatSearchDto> Chats { get; init; }
    public required List<UserSearchDto> Users { get; init; }
}