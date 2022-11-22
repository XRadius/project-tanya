namespace Tanya.Core
{
    public class Looper : EventWaitHandle
    {
        private readonly Timer _timer;
        private bool _isDisposed;

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

        #region Destructors

        ~Looper()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && !_isDisposed)
            {
                _timer.Dispose();
            }

            _isDisposed = true;
        }

        #endregion

        #region Methods

        private void SetInterval(TimeSpan duration)
        {
            _timer.Change(0, (int)duration.TotalMilliseconds);
        }

        #endregion
    }
}