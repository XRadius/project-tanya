using Microsoft.Extensions.Logging;

namespace Tanya.Logging
{
    public class Logger : ILogger
    {
        private readonly string _name;
        private readonly LoggerWriter _writer;

        #region Constructors

        public Logger(string name, LoggerWriter writer)
        {
            _name = name;
            _writer = writer;
        }

        #endregion

        #region Implementation of ILogger

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotSupportedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _writer.Enqueue($"[{DateTime.Now:s}] ({logLevel.ToString()[0]}) {_name} - {formatter(state, exception)}");
        }

        #endregion
    }
}