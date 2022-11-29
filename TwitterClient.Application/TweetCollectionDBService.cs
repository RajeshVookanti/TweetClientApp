using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using TwitterClient.Application.Helper;
using TwitterClient.Application.Model;
using TwitterClient.DataAccess;
using TwitterClient.Domain;
using TwitterClient.Infrastructure;

namespace TwitterClient.Application
{
    /// <summary>
    /// Service to capture the concurrent Tweet information in a thread safe manner
    /// Timer runs to flush the data in memory every 3 minutes to avoid out of memory issues
    /// Fush can be extended to save to a file or DB to persist the data
    /// </summary>
    public class TweetCollectionDBService : ITweetCollectionService
    {
        private readonly TwitterClientDbContext dbContext;
        private ConcurrentBag<Tweet> entities = new ConcurrentBag<Tweet>();
        private readonly ILogger<TweetCollectionDBService> logger;
        public TweetCollectionDBService(TwitterClientDbContext dbContext, ILogger<TweetCollectionDBService> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public async Task CollectTweetAsync(ITweet tweet)
        {
            var model = new TweetAnalysisModel(tweet.text);

            foreach (var tag in model.HashTags)
            {
                 entities.Add(new Tweet(tweet.id, tag));
            }

            await Task.CompletedTask;

        }
        
        public async Task CompleteAsync()
        {
            try
            {
                foreach (var entity in entities)
                {
                    await this.dbContext.AddAsync(entity);
                }

                await this.dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error while updating the tweets to DB");
            }
        }

        public IReadOnlyDictionary<string, int> HashTags => this.dbContext.Tweets.GroupBy(x => x.HashTag).
            ToDictionary(kv => kv.Key, kv => kv.Count());

        public int TotalTweets => this.dbContext.Tweets.GroupBy(x => x.TweetIdentifier).Count();
    }

}