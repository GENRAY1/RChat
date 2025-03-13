using System.Text;
using Dapper;

namespace RChat.Infrastructure.DataAccess.QueryBuilders;

public class QueryBuilder(
    string baseQuery,
    QueryPagination? pagination = null)
{
    private readonly StringBuilder _sql = new(baseQuery);
    private readonly StringBuilder _joins = new();
    private readonly StringBuilder _conditions = new();
    private readonly DynamicParameters _parameters = new();
    

    public void AddJoin(string joinClause)
    {
        _joins.Append(" ").Append(joinClause);
    }

    public void AddCondition(string condition)
    {
        if (_conditions.Length > 0)
            _conditions.Append(" AND ");

        _conditions.Append(condition);
    }

    public string BuildQuery()
    {
        _sql.Append(_joins);
        if (_conditions.Length > 0)
        {
            _sql.Append(" WHERE ").Append(_conditions);
        }

        if (pagination is not null)
        {
            _sql.AppendLine(" OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY");
            _parameters.Add("@Skip", pagination.Skip);
            _parameters.Add("@Take", pagination.Take);
        }

        return _sql.ToString();
    }

    public void AddParameter(string paramName, object paramValue)
    {
        _parameters.Add(paramName, paramValue);
    } 
    
    public DynamicParameters GetParameters()
    {
        return _parameters;
    }
}