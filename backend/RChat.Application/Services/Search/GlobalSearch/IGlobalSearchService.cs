namespace RChat.Application.Services.Search.GlobalSearch;

public interface IGlobalSearchService
{
    Task<GlobalSearchResult> Search(
        string query,
        int take,
        CancellationToken cancellationToken);
}