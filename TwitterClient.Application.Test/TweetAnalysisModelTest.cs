using TwitterClient.Application.Model;
using Xunit;
namespace TwitterClient.Application.Test
{
    public class TweetAnalysisModelTest
    {
        [Fact]
        public void validate_hashtag_parse()
        {
            var tweettext = @"Digital transformation in the #financialservices industry requires bold steps. That’s why @Microsoft and @UBS
                                are proud to expand our partnership, which will make #Azure the foundation for UBS’s industry-leading 
                                digital services for its clients and employees.";

            var sut = new TweetAnalysisModel(tweettext);

            Assert.Equal(2, sut.HashTags.Count());
            Assert.Equal("#financialservices", sut.HashTags[0]);
            Assert.Equal("#Azure", sut.HashTags[1]);

        }

    }
}