namespace TwitterClient.WebApi;

public class AppSettings
{
     public TwitterApi TwitterApi { get; set; } 
}

public class TwitterApi
{
    public string BearerToken { get; set; }

    public string DatabaseConnection { get; set; }
}
