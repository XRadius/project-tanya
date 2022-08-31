namespace Tanya.Core
{
    public class Limiter
    {
        private readonly TimeSpan _interval;
        private DateTime _nextTime;

        #region Constructors

        public Limiter(uint interval)
        {
            _interval = TimeSpan.FromMilliseconds(interval);
        }

        #endregion

        #region Methods

        public bool Update(DateTime frameTime)
        {
            if (frameTime < _nextTime) return false;
            SetNextTime(frameTime);
            return true;
        }

        private void SetNextTime(DateTime frameTime)
        {
            if (_interval.Ticks == 0) return;
            var currentTicks = frameTime.Ticks;
            var intervalTicks = _interval.Ticks;
            _nextTime = new DateTime(currentTicks - currentTicks % intervalTicks + intervalTicks);
        }

        #endregion
    }
}