using Elastic.Clients.Elasticsearch;
using RChat.Application.Services.Search.User;
using RChat.Infrastructure.Services.Search.Common;

namespace RChat.Infrastructure.Services.Search;

public class UserSearchService(ElasticsearchClient client) : IUserSearchService
{
    private readonly string _indexName = SearchIndexNames.User;
     
    public async Task<bool> IndexAsync(
        int id,
        UserDocument userDocument, CancellationToken cancellationToken){ 
        var response = await client.IndexAsync(
            userDocument,
            idx => idx.Index(_indexName).Id(id),
            cancellationToken);
        
        return response.IsValidResponse;
    }

    public async Task<bool> UpdateAsync(
        int id, 
        UpdateUserDocument userDocument, 
        CancellationToken cancellationToken)
    {
        var response = await client.UpdateAsync<UserDocument, UpdateUserDocument>(
            _indexName,
            id,
            u => u.Doc(userDocument),
            cancellationToken);

        return response.IsValidResponse;
    }
}