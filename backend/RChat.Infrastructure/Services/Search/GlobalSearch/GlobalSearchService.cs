using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using RChat.Application.Services.Search.GlobalSearch;
using RChat.Infrastructure.Services.Search.Common;

namespace RChat.Infrastructure.Services.Search.GlobalSearch;

public class GlobalSearchService(ElasticsearchClient client) : IGlobalSearchService
{
    private readonly string[] _indexes = [
        SearchIndexNames.Chat,
        SearchIndexNames.User
    ];

    private readonly string[] _chatFields = ["name"];
    private readonly string[] _userFields = ["firstname", "lastname", "username"];


    public async Task<GlobalSearchResult> Search(
        string query,
        int take,
        CancellationToken cancellationToken)
    {
        var searchResponse = await client.SearchAsync<object>(s => s
            .Index(_indexes)
            .Size(take)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        q => q.Bool(bb => bb
                            .Must(
                                q => 
                                    q.Term(t => t.Field("_index").Value(SearchIndexNames.User)),
                                q => q.MultiMatch(mm => mm
                                    .Fields(Fields.FromStrings(_userFields))
                                    .Type(TextQueryType.BoolPrefix)
                                    .Query(query)
                                )
                            )
                        ),
                        
                        q => q.Bool(bb => bb
                            .Must(
                                q => q.Term(t => t.Field("_index").Value(SearchIndexNames.Chat)),
                                q => q.MultiMatch(mm => mm
                                    .Fields(Fields.FromStrings(_chatFields)) 
                                    .Type(TextQueryType.BoolPrefix)
                                    .Query(query)
                                ),
                                q => q.Term(t => t
                                    .Field("isPrivate")
                                    .Value(false)
                                )
                            )
                            .MustNot(
                                q => q.Exists(e => e.Field("deletedAt"))
                            )
                        )
                    )
                )
            ), cancellationToken
        );
        
        var response = GlobalSearchMapper.Map(searchResponse);

        return response;
    }
}