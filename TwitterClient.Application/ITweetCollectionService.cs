using TwitterClient.Infrastructure;

namespace TwitterClient.Application
{
    public interface ITweetCollectionService
    {
        IReadOnlyDictionary<string, int> HashTags { get; }
        int TotalTweets { get; }

        Task CollectTweetAsync(ITweet tweet);

        Task CompleteAsync();
    }
}