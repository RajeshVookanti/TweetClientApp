using System;
using System.Text.RegularExpressions;

namespace TwitterClient.Application.Model
{
    public class TweetAnalysisModel
    {
        public List<string> HashTags { get; private set; }
        private Regex _regex = new Regex(@"#\w+");
        public TweetAnalysisModel(string tweetText)
        {
            HashTags = new List<string>();
            var collection = _regex.Matches(tweetText);
            if(collection != null)
            {
                HashTags = collection.ToList().Where(x => x != null && x.Value != null).Select(x => x.Value).ToList();
            }
        }
    }
}
