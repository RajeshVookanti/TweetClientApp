namespace TwitterClient.Application
{
    public interface ITweetAnalysisService
    {
        long GetTotalTweets();

        IReadOnlyCollection<Tuple<string, long>> GetTopTenHashTags();
    }
}