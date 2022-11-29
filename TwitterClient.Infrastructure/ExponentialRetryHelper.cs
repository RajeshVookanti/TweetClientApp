using System;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterClient.Infrastructure
{
    /// <summary>
    /// Calculates the length of time to delay before retrying an operation using an exponential backoff strategy.
    /// </summary>
    /// <remarks>
    /// Transient faults are errors that can be reasonably expected to occur and spontaneously resolve on their own. For example,
    /// when a connection to twitter stream api is broken, it may be because of a brief lost network connectivity. 
    /// In these cases, a retry-based approach to recover from the fault makes sense. However, we don't
    /// want to overwhelm the client or server in attempting to re-establish the connection, so we wait a little bit longer after
    /// every failed attempt. 
    /// </remarks>
    public struct ExponentialBackoff
    {
        private readonly int _delayMilliseconds;
        private readonly int _maxDelayMilliseconds;
        private int _pow;

        public ExponentialBackoff(int delayMilliseconds = 200,
            int maxDelayMilliseconds = 60000)
        {
            _delayMilliseconds = delayMilliseconds;
            _maxDelayMilliseconds = maxDelayMilliseconds;
            RetryCount = 0;
            _pow = 1;
        }

        public int RetryCount { get; private set; }

        public Task DelayAsync(CancellationToken cancellationToken = default)
        {
            ++RetryCount;

            if (RetryCount < 31)
            {
                _pow = _pow << 1; // m_pow = Pow(2, _retries - 1)
            }

            int delay = Math.Min(_delayMilliseconds * (_pow - 1) / 2,
                _maxDelayMilliseconds);

            return Task.Delay(delay, cancellationToken);
        }
    }
}
