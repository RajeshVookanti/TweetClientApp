namespace TwitterClient.WebApi.Model;

public class TwitterMetricsDto
{
    public long TotalTweets { get; set; }

    public List<string> TopHashtags { get; set; }
}
