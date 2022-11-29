using Microsoft.Extensions.DependencyInjection;
using TwitterClient.Application.Handlers;
using TwitterClient.Infrastructure;
using System.Threading.Tasks.Dataflow;

namespace TwitterClient.Application;
/// <summary>
/// Twitter stream subscriber, subscribe to the tweet life cycle hooks and publishes the event to the other subscribers
/// </summary>
public class TwitterStreamSubscriber : ITweetStreamingLifeCycleHooks
{
    private readonly IServiceProvider serviceProvider;
    public TwitterStreamSubscriber(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public async Task TweetReceivedAsync(IList<ITweet> tweets)
    {
        using var scope = serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<ITweetHandler<ITweet>>();
        var blocks = new List<ActionBlock<ITweet>>();
        foreach(var handler in handlers)
        {
            var block = new ActionBlock<ITweet>(
                                data => handler.HandleAsync(data),
                                new ExecutionDataflowBlockOptions
                                {
                                    BoundedCapacity = tweets.Count(),
                                    MaxDegreeOfParallelism = Environment.ProcessorCount
                                });
            foreach(var tweet in tweets)
            {
                await block.SendAsync(tweet).ConfigureAwait(false);
            }
            block.Complete();
            blocks.Add(block);
        }

        await Task.WhenAll(blocks.Select(x => x.Completion));

        foreach (var handler in handlers)
        {
            await handler.CompleteAsync().ConfigureAwait(false);
        }
    }
}
