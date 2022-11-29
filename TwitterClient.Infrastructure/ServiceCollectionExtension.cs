using Microsoft.Extensions.DependencyInjection;

namespace TwitterClient.Infrastructure;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCommunicationServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<ITwitterStream, TwitterStream>();
        return services;
    }
}
