namespace RChat.Application.Services.Search;

public class SearchResult<T>
{
    public int Id { get; set; }
    public string Index { get; set; } = null!;
    public T Document { get; set; } = default!;
}