namespace TwitterClient.Infrastructure;
public interface ITweetStreamingLifeCycleHooks
{
    Task TweetReceivedAsync(IList<ITweet> tweets);
}
