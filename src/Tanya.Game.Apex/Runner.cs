using Tanya.Core;
using Tanya.Game.Apex.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex
{
    public class Runner : IDisposable
    {
        private readonly Config _config;
        private readonly CancellationTokenSource _cts;
        private readonly List<IFeature> _features;
        private readonly State _state;
        private bool _isDisposed;

        #region Constructors

        private Runner(Config config, IEnumerable<IFeature> features, State state)
        {
            _config = config;
            _cts = new CancellationTokenSource();
            _features = features.ToList();
            _state = state;
        }

        public static Runner Create(Config config, IEnumerable<IFeature> features, State state)
        {
            var runner = new Runner(config, features, state);
            var thread = new Thread(runner.Process) { IsBackground = true };
            thread.Start();
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
            }

            _isDisposed = true;
        }

        #endregion

        #region Methods

        private void Process()
        {
            var looper = new Looper();

            while (looper.Tick(_config.FramesPerSecond, _cts.Token))
            {
                var frameTime = DateTime.UtcNow;
                _state.Update(frameTime);
                _features.ForEach(x => x.Tick(frameTime, _state));
            }
        }

        #endregion
    }
}