using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Tanya.Logging
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Logger> _loggers;
        private readonly LoggerWriter _writer;

        #region Constructors

        public LoggerProvider()
        {
            _loggers = new ConcurrentDictionary<string, Logger>(StringComparer.InvariantCultureIgnoreCase);
            _writer = LoggerWriter.Create();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _loggers.Clear();
            _writer.Dispose();
            GC.SuppressFinalize(this);
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