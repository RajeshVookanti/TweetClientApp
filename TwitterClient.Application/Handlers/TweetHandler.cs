
using TwitterClient.Infrastructure;

namespace TwitterClient.Application.Handlers
{
    public class TweetHandler : ITweetHandler<ITweet>
    {
        private readonly ITweetCollectionService tweetCollectionService;

        public TweetHandler(ITweetCollectionService tweetCollectionService)
        {
            this.tweetCollectionService = tweetCollectionService ?? throw new ArgumentNullException(nameof(tweetCollectionService));
        }

        public async Task HandleAsync(ITweet tweet)
        {
            await this.tweetCollectionService.CollectTweetAsync(tweet);
            
        }

        public async Task CompleteAsync()
        {
            await this.tweetCollectionService.CompleteAsync();

        }
    }
}