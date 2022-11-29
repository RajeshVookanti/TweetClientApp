namespace TwitterClient.Domain
{
    public class Tweet
    {
        public int Id { get; set; }

        public string TweetIdentifier { get; set; }
        public string HashTag { get; set; }

        public Tweet()
        {

        }
        public Tweet(string id, string hashTag)
        {
            TweetIdentifier = id;
            HashTag = hashTag;
        }
    }

}