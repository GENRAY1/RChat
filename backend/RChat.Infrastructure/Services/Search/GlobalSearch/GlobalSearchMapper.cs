using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using RChat.Application.Services.Search;
using RChat.Application.Services.Search.Chat;
using RChat.Application.Services.Search.GlobalSearch;
using RChat.Application.Services.Search.User;
using RChat.Infrastructure.Services.Search.Common;

namespace RChat.Infrastructure.Services.Search.GlobalSearch;

public static class GlobalSearchMapper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static GlobalSearchResult Map(SearchResponse<object> response)
    {
        var result = new GlobalSearchResult();

        foreach (var hit in response.Hits)
        {
            var json = hit.Source?.ToString();

            if (!int.TryParse(hit.Id, out var id)) continue;
            
            if (string.IsNullOrEmpty(json)) continue;

            switch (hit.Index)
            {
                case SearchIndexNames.User:
                    var user = JsonSerializer.Deserialize<UserDocument>(json, JsonOptions);
                    if (user != null)
                        result.Users.Add(new SearchResult<UserDocument>
                        {
                            Id = id,
                            Index = hit.Index,
                            Document = user
                        });
                    break;

                case SearchIndexNames.Chat:
                    var chat = JsonSerializer.Deserialize<ChatDocument>(json, JsonOptions);
                    if (chat != null)
                        result.Chats.Add(new SearchResult<ChatDocument>
                        {
                            Id = id,
                            Index = hit.Index,
                            Document = chat
                        });
                    break;
            }
        }

        return result;
    }
}