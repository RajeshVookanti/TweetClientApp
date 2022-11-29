using Microsoft.Extensions.DependencyInjection;
using TwitterClient.Infrastructure;
using TwitterClient.Application.Handlers;

namespace TwitterClient.Application;
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTweetAnalysisServices(this IServiceCollection services)
    {
        services.AddSingleton<ITweetCollectionService, TweetCollectionInMemoryService>(); // this service colects the tweets into memory and read it from there

        //services.AddTransient<ITweetCollectionService, TweetCollectionDBService>(); // this service colects the tweets into a DB and read it from there
        services.AddTransient<ITweetAnalysisService, TweetAnalysisService>();
        services.AddTransient<ITweetHandler<ITweet>, TweetHandler>();     
        services.AddTransient<ITweetStreamingLifeCycleHooks, TwitterStreamSubscriber>();
        services.AddCommunicationServices();
        services.AddSingleton<ITweetStreamService, TweetStreamService>();        
        return services;
    }
}
