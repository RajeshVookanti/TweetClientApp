using System.Collections.Concurrent;
using TwitterClient.Infrastructure;

namespace TwitterClient.Application 
{
    /// <summary>
    /// Service to connect to the infracture streaming service and start /cancel the streaming
    /// This can be extended to restart the streaming
    /// </summary>
    public class TweetStreamService : ITweetStreamService
    {
        private readonly ITwitterStream twitterStreamService;
        private readonly CancellationTokenSource cancellationToken;
        public TweetStreamService(ITwitterStream twitterStreamService)
        {
            this.twitterStreamService = twitterStreamService ?? throw new ArgumentNullException(nameof(twitterStreamService));
            this.cancellationToken = new CancellationTokenSource();
        }

        public void StartStreaming(string token)
        {
            this.twitterStreamService.StreamAsync(token,this.cancellationToken.Token).ConfigureAwait(false);
        }

        public void CancelStreaming()
        {
            this.cancellationToken.Cancel();
        }

    }

}