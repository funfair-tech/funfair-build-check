using System;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Services
{
    public sealed class LoggerProxy<TLogClass> : ILogger<TLogClass>
    {
        private readonly ILogger _diagnosticLogger;

        public LoggerProxy(ILogger diagnosticLogger)
        {
            this._diagnosticLogger = diagnosticLogger ?? throw new ArgumentNullException(nameof(diagnosticLogger));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this._diagnosticLogger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this._diagnosticLogger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this._diagnosticLogger.BeginScope(state);
        }
    }
}