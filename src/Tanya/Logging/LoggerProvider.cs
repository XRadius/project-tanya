using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Tanya.Logging
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Logger> _loggers;
        private readonly LoggerWriter _writer;
        private bool _isDisposed;

        #region Constructors

        public LoggerProvider()
        {
            _loggers = new ConcurrentDictionary<string, Logger>(StringComparer.InvariantCultureIgnoreCase);
            _writer = LoggerWriter.Create();
        }

        #endregion

        #region Destructors

        ~LoggerProvider()
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
                _loggers.Clear();
                _writer.Dispose();
            }

            _isDisposed = true;
        }

        #endregion

        #region Implementation of ILoggerProvider

        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name, _ => new Logger(name, _writer));
        }

        #endregion
    }
}