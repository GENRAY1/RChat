using FluentMigrator.Runner;

namespace RChat.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrator.MigrateUp();
    }
}