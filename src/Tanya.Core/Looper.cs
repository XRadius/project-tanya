namespace Tanya.Core
{
    public class Looper
    {
        private int _currentFrame;
        private int? _framesPerSecond;
        private TimeSpan _interval;
        private DateTime _startTime;

        #region Methods

        public bool Tick(int framesPerSecond, CancellationToken token)
        {
            if (_framesPerSecond != framesPerSecond)
            {
                _currentFrame = 0;
                _framesPerSecond = Math.Max(1, framesPerSecond);
                _interval = TimeSpan.FromMilliseconds(1000f / _framesPerSecond.Value);
                _startTime = DateTime.UtcNow;
            }

            while (!token.IsCancellationRequested)
            {
                var elapsedTime = DateTime.UtcNow - _startTime;
                var targetFrame = Math.Floor(elapsedTime / _interval);

                if (_currentFrame < targetFrame)
                {
                    _currentFrame++;
                    return true;
                }

                Thread.Sleep(1);
            }

            return false;
        }

        #endregion
    }
}