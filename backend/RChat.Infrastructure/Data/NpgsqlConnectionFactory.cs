using Microsoft.Extensions.Configuration;
using Npgsql;

namespace RChat.Infrastructure.Data;

public class NpgsqlConnectionFactory(string connectionString) 
    : IDbConnectionFactory
{
    public NpgsqlConnection Create()
    {
        return new NpgsqlConnection(connectionString);
    }
}