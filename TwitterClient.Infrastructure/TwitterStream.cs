using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using TwitterClient.Infrastructure;

namespace TwitterClient.Infrastructure;
public class TwitterStream : ITwitterStream
{
    private const string SampleStreamngUrl = "https://api.twitter.com/2/tweets/sample/stream";
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<TwitterStream> logger;
    private readonly ITweetStreamingLifeCycleHooks subscriber;
    private readonly CancellationTokenSource disconnectedTokenSource = new CancellationTokenSource();
    // Create a new backoff helper to calc the wait times - max wait 60 seconds
    private readonly ExponentialBackoff backoff = new ExponentialBackoff(1000, 60000);
    public TwitterStream(IHttpClientFactory httpClientFactory,ILogger<TwitterStream> logger, ITweetStreamingLifeCycleHooks subscriber)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
    }

    /// <summary>
    /// Connects to Twitter sample Streams API and read from the response continously until cancelled
    /// Exponentially reconnects if there is any exception
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StreamAsync(string token,CancellationToken cancellationToken)
    {
        var client = this.httpClientFactory.CreateClient();
        UriBuilder uriBuilder = new UriBuilder(SampleStreamngUrl);
        uriBuilder.Query = "tweet.fields=id,text";
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri.ToString());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Authorization", "Bearer " + token);

                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                this.logger.LogInformation("Successfully connected to twitter sample streaming api, attempt # "+ this.backoff.RetryCount);

                var stream = await response.Content.ReadAsStreamAsync()
                    .ConfigureAwait(false);

                long tweetNotificationCounter = 0;
                using (var reader = new StreamReader(stream))
                {
                    var tweets = new List<ITweet>();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);

                        if (!string.IsNullOrEmpty(line))
                        {
                            this.logger.LogDebug("New tweet received: {tweet}", line.Replace("•", ""));
                            using var document = JsonDocument.Parse(line);
                            if (document.RootElement.TryGetProperty("data", out JsonElement data))
                            {
                                var tweet = data.Deserialize<SampleTweet>();

                                if (tweet != null)
                                {
                                    tweets.Add(tweet);
                                }
                            }
                            if (tweets.Count() == 100)
                            {
                                tweetNotificationCounter += 100;
                                await this.subscriber.TweetReceivedAsync(tweets).ConfigureAwait(false);
                                tweets.Clear();
                            }
                        }
                    }

                    if (tweets.Count() > 0)
                    {
                        await this.subscriber.TweetReceivedAsync(tweets).ConfigureAwait(false);
                        tweets.Clear();
                    }

                    this.logger.LogInformation("Successfully cancelled streaming the twitter sample api");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while streaming the tweets");
                await this.backoff.DelayAsync(this.disconnectedTokenSource.Token).ConfigureAwait(false);
            }
        }
    
    }
    
}
