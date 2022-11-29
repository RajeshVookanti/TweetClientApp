namespace TwitterClient.Application 
{
    public interface ITweetStreamService
    {
        void StartStreaming(string token);

        void CancelStreaming();
    }
}