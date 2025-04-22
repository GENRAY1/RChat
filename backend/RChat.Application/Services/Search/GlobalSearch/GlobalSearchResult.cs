using RChat.Application.Services.Search.Chat;
using RChat.Application.Services.Search.User;

namespace RChat.Application.Services.Search.GlobalSearch;

public class GlobalSearchResult
{
    public List<SearchResult<UserDocument>> Users { get; init; } = [];

    public List<SearchResult<ChatDocument>> Chats { get; init; } = [];
}