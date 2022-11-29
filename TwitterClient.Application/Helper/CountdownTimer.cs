
namespace TwitterClient.Application.Helper
{
    /// <summary>
    /// Provides a resettable timer that invokes a callback at given intervals.
    /// </summary>
    /// <remarks>
    public class CountdownTimer : IDisposable
    {
        private readonly TimerCallback _callback;
        private readonly TimeSpan _interval;
        private Timer _timer;

        /// <summary>
        /// Initializes a new <see cref="CountdownTimer"/> but does not start it.
        /// </summary>
        /// <param name="callback">The callback to invoke every time the interval elapses.</param>
        /// <param name="interval">The interval at which the callback should be invoked.</param>
        public CountdownTimer(TimerCallback callback, TimeSpan interval)
        {
            if (interval == TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(interval));
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _interval = interval;
        }

        /// <summary>
        /// Starts the countdown timer, which will invoke the callback provided every time the given
        /// interval elapses without ResetCountdown() being called. The first invocation will occur once
        /// the initial interval has elapsed.
        /// </summary>
        public void Start()
        {
            if (_timer != null)
                return; // Already started

            // Create a new timer that will fire after _interval and then continue to fire every _interval
            // (unless stop or ResetCountdown is called)
            _timer = new Timer(_callback, null, _interval, _interval);
        }

        /// <summary>
        /// Stops the countdown timer and releases resources.
        /// </summary>
        public void Stop()
        {
            if (_timer == null)
                return; // Already stopped

            _timer.Dispose();
            _timer = null;
        }

        /// <summary>
        /// Resets the countdown timer to the full interval.
        /// </summary>
        public void ResetCountdown()
        {
            if (_timer == null)
                return; // We're stopped

            _timer.Change(_interval, _interval);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    this.Stop();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}
