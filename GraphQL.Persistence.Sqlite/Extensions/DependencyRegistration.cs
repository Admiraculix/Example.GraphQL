using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Persistence.Sqlite.Extensions;

public static class DependencyRegistration
{
    public static IServiceCollection AddPersistenceSqliteRegistration(this IServiceCollection services, IConfiguration cofiguration)
    {
        string connectionString = cofiguration.GetConnectionString("Sqlite");
        services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionString).LogTo(Console.WriteLine));

        return services;
    }
}