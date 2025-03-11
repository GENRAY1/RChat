using Npgsql;

namespace RChat.Infrastructure.DataAccess.Connections;

public class NpgsqlConnectionFactory(string connectionString) 
    : IDbConnectionFactory
{
    public async Task<NpgsqlConnection> Create()
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        return connection;
    }
}