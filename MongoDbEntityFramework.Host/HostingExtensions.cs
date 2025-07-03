using Microsoft.Extensions.DependencyInjection;
using MongoDbEntityFramework.Settings;

namespace MongoDbEntityFramework.Host;

public static class HostingExtensions
{
    public static void AddMongoDbContext<TContext>(this IServiceCollection services, DbSettings dbSettings)
        where TContext : DbContext, new()
    {
        services.AddSingleton<TContext>(provider => 
        {
            TContext context = new();
            context.Initialize(dbSettings);
            return context;
        });
    }
    
}