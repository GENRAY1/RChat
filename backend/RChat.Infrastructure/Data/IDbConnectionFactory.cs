using Npgsql;

namespace RChat.Infrastructure.Data;

public interface IDbConnectionFactory
{
    NpgsqlConnection Create();
}