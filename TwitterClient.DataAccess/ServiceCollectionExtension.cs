using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TwitterClient.DataAccess;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDBServices(this IServiceCollection services,string connection)
    {
        return services.AddDbContext<TwitterClientDbContext>(options =>
                options.UseSqlite(connection));
    }
}
