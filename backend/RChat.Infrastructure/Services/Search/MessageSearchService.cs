using Elastic.Clients.Elasticsearch;
using RChat.Application.Services.Search.Message;
using RChat.Infrastructure.Services.Search.Common;

namespace RChat.Infrastructure.Services.Search;

public class MessageSearchService(ElasticsearchClient client) : IMessageSearchService
{
    private readonly string _indexName = SearchIndexNames.Message;

    public async Task<bool> IndexAsync(
        int id,
        MessageDocument messageDocument,
        CancellationToken cancellationToken){ 
        var response = await client.IndexAsync(
            messageDocument, 
            idx => idx.Index(_indexName),
            cancellationToken);
        
        return response.IsValidResponse;
    }

    public async Task<bool> UpdateAsync(
        int id, 
        UpdateMessageDocument messageDocument, 
        CancellationToken cancellationToken)
    {
        var response = await client.UpdateAsync<MessageDocument, UpdateMessageDocument>(
            _indexName,
            id,
            u => u.Doc(messageDocument),
            cancellationToken);
        
        return response.IsValidResponse;
    }
}