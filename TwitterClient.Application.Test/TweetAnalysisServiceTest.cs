using TwitterClient.Infrastructure;
using Xunit;
namespace TwitterClient.Application .Test
{
    public class TweetAnalysisServiceTest
    {
        [Fact]
        public void validate_collection_inmemory()
        {
            var collectionService = new TweetCollectionInMemoryService();

            var data = new List<ITweet>()
            {
                new SampleTweet() { id =  "1", text = string.Join("","#ht1", "#ht2", "#ht3", "#ht4", "#ht5", "#ht6", "#ht7", "#ht8", "#ht9", "#ht10" ) }
                ,new SampleTweet() { id = "1", text = string.Join("","#ht2", "#ht3", "#ht4", "#ht5", "#ht6", "#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht3", "#ht4", "#ht5", "#ht6", "#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht4", "#ht5", "#ht6", "#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht5", "#ht6", "#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht6", "#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht7", "#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht8", "#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = string.Join("","#ht9", "#ht10") }
                ,new SampleTweet() { id =  "1", text = "#ht10" }
            };

            Parallel.ForEach(data, new ParallelOptions() { MaxDegreeOfParallelism = 10} , async (tweet) => { await collectionService.CollectTweetAsync(tweet); });

            var sut = new TweetAnalysisService(collectionService);

            var hashtags = sut.GetTopTenHashTags().ToList();

            Assert.Equal(10, hashtags.Count());

            foreach(var num in Enumerable.Range(1,10).OrderByDescending(x => x))
            {
                Assert.Equal("#ht"+num, hashtags[10-num].Item1);
                Assert.Equal(num, hashtags[10 - num].Item2);
            }

            Assert.Equal(10, sut.GetTotalTweets());


        } 
    }
}