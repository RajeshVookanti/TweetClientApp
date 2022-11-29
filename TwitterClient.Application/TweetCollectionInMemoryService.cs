using System.Collections.Concurrent;
using TwitterClient.Application.Helper;
using TwitterClient.Application.Model;
using TwitterClient.Infrastructure;

namespace TwitterClient.Application
{
    /// <summary>
    /// Service to capture the concurrent Tweet information in a thread safe manner
    /// Timer runs to flush the data in memory every 3 minutes to avoid out of memory issues
    /// Fush can be extended to save to a file or DB to persist the data
    /// </summary>
    public class TweetCollectionInMemoryService : ITweetCollectionService
    {
        private ConcurrentDictionary<string, int> hashtags = new ConcurrentDictionary<string, int>();

        private int totalTweets = 0;

        private readonly CountdownTimer timer;
        public TweetCollectionInMemoryService()
        {
            this.timer = new CountdownTimer(this.Flush, TimeSpan.FromMilliseconds(3 * 60 * 1000));
            this.timer.Start();
        }

        public async Task CollectTweetAsync(ITweet tweet)
        {
            var model = new TweetAnalysisModel(tweet.text);

            this.CollectHashTags(model.HashTags);
            this.CollectTweets();

            await Task.CompletedTask;

        }

        public async Task CompleteAsync()
        {
            await Task.CompletedTask;
        }

        private void CollectHashTags(List<string> tags)
        {
            foreach (var tag in tags)
            {
                this.hashtags.AddOrUpdate(tag, key => { return 1; }, (key, qty) => qty + 1);
            }
        }

        private void CollectTweets()
        {
            Interlocked.Increment(ref this.totalTweets);
        }

       
        private void Flush(object state)
        {
            this.hashtags.Clear();
            this.totalTweets = 0;
        }

        public IReadOnlyDictionary<string, int> HashTags => this.hashtags.ToDictionary(kv => kv.Key, kv => kv.Value);

        public int TotalTweets => this.totalTweets;
    }

}