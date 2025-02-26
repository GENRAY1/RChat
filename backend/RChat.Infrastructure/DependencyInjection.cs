using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RChat.Infrastructure.Data;

namespace RChat.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") 
                                  ?? throw new ArgumentNullException(nameof(configuration));
        
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres() 
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DependencyInjection).Assembly).For.Migrations()) 
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}