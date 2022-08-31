using Microsoft.Extensions.Logging;
using Tanya.Driver.Interfaces;

namespace Tanya.Game.Apex.Core
{
    public abstract class Context : IDriver, ILogger
    {
        private readonly IDriver _driver;
        private readonly ILogger _logger;

        #region Constructors

        protected Context(IDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        #endregion

        #region Implementation of IDriver

        public bool Read(ulong address, byte[] buffer, int count)
        {
            return _driver.Read(address, buffer, count);
        }

        public bool Write(ulong address, byte[] buffer, int count)
        {
            return _driver.Write(address, buffer, count);
        }

        #endregion

        #region Implementation of ILogger

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        #endregion
    }
}