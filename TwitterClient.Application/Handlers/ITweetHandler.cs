using TwitterClient.Infrastructure;

namespace TwitterClient.Application.Handlers
{
    public interface ITweetHandler<T> where T : ITweet
    {
        Task HandleAsync(T tweet);

        Task CompleteAsync();
    }
}