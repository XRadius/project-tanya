namespace Tanya.Core
{
    public class Looper : EventWaitHandle
    {
        private readonly Timer _timer;

        #region Constructors

        private Looper() : base(false, EventResetMode.AutoReset)
        {
            _timer = new Timer(_ => Set());
        }

        public static Looper Create(TimeSpan interval)
        {
            var looper = new Looper();
            looper.SetInterval(interval);
            return looper;
        }

        #endregion

        #region Methods

        private void SetInterval(TimeSpan duration)
        {
            _timer.Change(0, (int)duration.TotalMilliseconds);
        }

        #endregion

        #region Overrides of WaitHandle

        protected override void Dispose(bool explicitDisposing)
        {
            if (explicitDisposing)
            {
                _timer.Dispose();
            }

            base.Dispose(explicitDisposing);
        }

        #endregion
    }
}