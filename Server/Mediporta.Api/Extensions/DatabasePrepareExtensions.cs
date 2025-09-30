using Mediporta.Data;
using Microsoft.EntityFrameworkCore;

namespace Mediporta.Api.Extensions;

public static class DatabasePrepareExtensions
{
    public static IServiceProvider PrepareDatabase(this IServiceProvider serviceProvider)
    {
        // Due to using Sqlite InMemory database for testing, we need to ensure that the database is created and migrated each time.
        // ToDo: For future - detecting state installation vs wake up and prevent erase of data on wake up
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MediportaDbContext>();

        var dataSourcePath = Path.GetFullPath(dbContext.Database.GetDbConnection().DataSource);
        if (File.Exists(dataSourcePath))
            File.Delete(dataSourcePath);

        dbContext.Database.Migrate();
        return serviceProvider;
    }
}