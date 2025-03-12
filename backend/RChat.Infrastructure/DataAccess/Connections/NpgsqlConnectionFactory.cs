using Npgsql;

namespace RChat.Infrastructure.DataAccess.Connections;

public class NpgsqlConnectionFactory(string connectionString) 
    : IDbConnectionFactory
{
    public async Task<NpgsqlConnection> CreateAsync()
    {
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        return connection;
    }
}