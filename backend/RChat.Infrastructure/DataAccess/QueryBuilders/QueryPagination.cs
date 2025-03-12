namespace RChat.Infrastructure.DataAccess.QueryBuilders;

public class QueryPagination(int skip, int take)
{
    public int Skip { get; } = skip;
    
    public int Take { get; } = take;
}