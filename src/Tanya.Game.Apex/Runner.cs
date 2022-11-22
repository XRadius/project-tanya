using Tanya.Core;
using Tanya.Game.Apex.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex
{
    public class Runner : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly List<IFeature> _features;
        private readonly Looper _looper;
        private readonly State _state;
        private bool _isDisposed;

        #region Constructors

        private Runner(Config config, IEnumerable<IFeature> features, State state)
        {
            _cts = new CancellationTokenSource();
            _features = features.ToList();
            _looper = Looper.Create(TimeSpan.FromMilliseconds(1000f / config.FramesPerSecond));
            _state = state;
        }

        public static Runner Create(Config config, IEnumerable<IFeature> features, State state)
        {
            var runner = new Runner(config, features, state);
            Task.Factory.StartNew(runner.Process, TaskCreationOptions.LongRunning);
            return runner;
        }

        #endregion

        #region Destructors

        ~Runner()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _cts.Cancel();
                _looper.Dispose();
            }

            _isDisposed = true;
        }

        #endregion

        #region Methods

        private void Process()
        {
            var events = new[] { _looper, _cts.Token.WaitHandle };

            while (WaitHandle.WaitAny(events) == 0)
            {
                var frameTime = DateTime.UtcNow;
                _state.Update(frameTime);
                _features.ForEach(x => x.Tick(frameTime, _state));
            }
        }

        #endregion
    }
}