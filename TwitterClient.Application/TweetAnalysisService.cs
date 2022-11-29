using System.Collections.Concurrent;
using TwitterClient.Infrastructure;

namespace TwitterClient.Application
{
    /// <summary>
    /// Service to analyze the tweet colection and return the Metrics 
    /// </summary>
    public class TweetAnalysisService : ITweetAnalysisService
    {
        private readonly ITweetCollectionService tweetCollectionService;

        public TweetAnalysisService(ITweetCollectionService tweetCollectionService)
        {
            this.tweetCollectionService = tweetCollectionService ?? throw new ArgumentNullException(nameof(tweetCollectionService));
        }
         
        public long GetTotalTweets() { return this.tweetCollectionService.TotalTweets; }

        public IReadOnlyCollection<Tuple<string, long>> GetTopTenHashTags()
        {
            return this.tweetCollectionService.HashTags
            .GroupBy(x => x)
            .Select(m => new Tuple<string, long>( m.Key.Key, m.Key.Value ))
            .OrderByDescending(x => x.Item2).Take(10).ToList().AsReadOnly();
        }
    }
     
}