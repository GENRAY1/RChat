using Elastic.Clients.Elasticsearch;
using RChat.Application.Services.Search.Chat;
using RChat.Infrastructure.Services.Search.Common;

namespace RChat.Infrastructure.Services.Search;

public class ChatSearchService(ElasticsearchClient client) : IChatSearchService
{
    private readonly string _indexName = SearchIndexNames.Chat;
    
    public async Task<bool> IndexAsync(
        int id,
        ChatDocument chatDocument,
        CancellationToken cancellationToken) { 
        var response = await client.IndexAsync(
            chatDocument, 
            idx => idx.Index(_indexName).Id(id),
            cancellationToken);
        
        return response.IsValidResponse;
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateChatDocument chatDocument,
        CancellationToken cancellationToken)
    {
        var response = await client.UpdateAsync<ChatDocument, UpdateChatDocument>(
            _indexName,
            id,
            u => u.Doc(chatDocument),
            cancellationToken);
        
        return response.IsValidResponse;
    }
}