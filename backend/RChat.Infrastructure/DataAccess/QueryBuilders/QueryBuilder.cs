using System.Text;
using Dapper;
using RChat.Application.Common;
using RChat.Application.Common.Sorting;

namespace RChat.Infrastructure.DataAccess.QueryBuilders;

public class QueryBuilder(string baseQuery)
{
    private readonly StringBuilder _sql = new(baseQuery);
    private readonly StringBuilder _joins = new();
    private readonly StringBuilder _conditions = new();
    private readonly DynamicParameters _parameters = new();
    private string? _orderBySql;
    private string? _paginationSql;
    
    
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

    public void AddPagination(PaginationDto pagination)
    {
        _paginationSql = " OFFSET (@Skip) ROWS FETCH NEXT (@Take) ROWS ONLY";
        _parameters.Add("@Skip", pagination.Skip);
        _parameters.Add("@Take", pagination.Take);
    }
    
    public void AddSorting<T>(Dictionary<T, string> sortingColumnDbMapping, SortingDto<T> sorting) 
        where T : Enum 
    {
        if (sortingColumnDbMapping.TryGetValue(sorting.Column, out var columnName))
        {
            _orderBySql = $" ORDER BY {columnName} {sorting.Direction.ToString()}";
            return;
        }
        
        throw new ArgumentException("Invalid sorting column " + sorting.Column);
    }

    public void AddSorting(string column, SortingDirection direction) =>
        _orderBySql = $" ORDER BY {column} {direction.ToString()}";
    
    public void AddSorting<T>(Dictionary<T, string> sortingColumnDbMapping, MultiSortingDto<T> sorting) 
        where T : Enum
    {
        string[] columnNames = sorting.Columns
            .Select(columnEnum =>
                sortingColumnDbMapping.GetValueOrDefault(columnEnum)
                ?? throw new ArgumentException("Invalid sorting column " + columnEnum))
            .ToArray();

        _orderBySql = $" ORDER BY {string.Join(", ", columnNames)} {sorting.Direction.ToString()}";
    }

    public string BuildQuery()
    {
        _sql.Append(_joins);
        if (_conditions.Length > 0)
        {
            _sql.Append(" WHERE ").Append(_conditions);
        }

        if (_orderBySql is not null)
        {
           _sql.AppendLine(_orderBySql); 
        }
        
        if (_paginationSql is not null)
        {
            _sql.AppendLine(_paginationSql);
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