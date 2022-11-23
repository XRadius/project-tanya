using System.Collections.Concurrent;
using System.Diagnostics;

namespace Tanya.Logging
{
    public class LoggerWriter : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly AutoResetEvent _event;
        private readonly ConcurrentQueue<string> _queue;
        private bool _isDisposed;

        #region Constructors

        private LoggerWriter()
        {
            _cts = new CancellationTokenSource();
            _event = new AutoResetEvent(false);
            _queue = new ConcurrentQueue<string>();
        }

        public static LoggerWriter Create()
        {
            var writer = new LoggerWriter();
            Task.Factory.StartNew(writer.ProcessAsync, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return writer;
        }

        #endregion

        #region Destructors

        ~LoggerWriter()
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
                _event.Dispose();
            }

            _isDisposed = true;
        }

        #endregion

        #region Methods

        public void Enqueue(string value)
        {
            _queue.Enqueue(value);
            _event.Set();
        }

        private async Task ProcessAsync()
        {
            await using var fileStream = File.Open("Tanya.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            await using var writer = new StreamWriter(fileStream);

            var isDebugging = Debugger.IsAttached;
            var events = new[] { _event, _cts.Token.WaitHandle };

            while (WaitHandle.WaitAny(events) == 0)
            {
                while (_queue.TryDequeue(out var value))
                {
                    if (isDebugging) Console.WriteLine(value);
                    await writer.WriteLineAsync(value).ConfigureAwait(false);
                    await writer.FlushAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}