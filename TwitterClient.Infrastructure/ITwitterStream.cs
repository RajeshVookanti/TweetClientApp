namespace TwitterClient.Infrastructure;
public interface ITwitterStream
{
    Task StreamAsync(string token,CancellationToken cancellationToken);
}
