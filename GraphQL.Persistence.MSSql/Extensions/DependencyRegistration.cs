using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Persistence.MSSql.Extensions;

public static class DependencyRegistration
{
    public static IServiceCollection AddPersistenceMSSqlRegistration(this IServiceCollection services, IConfiguration cofiguration)
    {
        string connectionString = cofiguration.GetConnectionString("Default");
        services.
            AddPooledDbContextFactory<SchoolDbContext>(options => options
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine)
            );

        return services;
    }
}