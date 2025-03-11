using Npgsql;

namespace RChat.Infrastructure.DataAccess.Connections;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> Create();
}