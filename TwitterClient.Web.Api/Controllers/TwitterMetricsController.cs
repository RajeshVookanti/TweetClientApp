using Microsoft.AspNetCore.Mvc;
using TwitterClient.Infrastructure;
using TwitterClient.Application;
using TwitterClient.WebApi.Model;

namespace TwitterCient.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TwitterMetricsController : ControllerBase
{
    private readonly ITweetAnalysisService twitterAnalysisService;
    private readonly ITweetStreamService tweetStreamService;
    public TwitterMetricsController(ITweetAnalysisService twitterAnalysisService, 
        ITweetStreamService tweetStreamService)
    {
        this.twitterAnalysisService = twitterAnalysisService ?? throw new ArgumentNullException(nameof(twitterAnalysisService));
        this.tweetStreamService = tweetStreamService ?? throw new ArgumentNullException(nameof(tweetStreamService));
    }
    [HttpGet]
    public  TwitterMetricsDto Get()
    {
        return new TwitterMetricsDto()
        {
            TopHashtags = this.twitterAnalysisService.GetTopTenHashTags().Select(x => { return $"{x.Item1} with {x.Item2} entres"; }).ToList(),
            TotalTweets = this.twitterAnalysisService.GetTotalTweets()
        };
    }

    [HttpPost("cancel")]
    public void Cancel()
    {
        this.tweetStreamService.CancelStreaming();
    }
}
