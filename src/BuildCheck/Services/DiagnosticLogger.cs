using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace BuildCheck.Services
{
    public sealed class DiagnosticLogger : IDiagnosticLogger
    {
        private readonly bool _warningsAsErrors;
        private long _errors;

        public DiagnosticLogger(bool warningsAsErrors)
        {
            this._warningsAsErrors = warningsAsErrors;
        }

        public long Errors => this._errors;

        public bool IsErrored => this.Errors > 0;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (this.IsWarningAsError(logLevel))
            {
                // ReSharper disable once TailRecursiveCall - this is not tail recursive despite what R# thinks
                this.Log(LogLevel.Error, eventId, state, exception, formatter);

                return;
            }

            if (logLevel == LogLevel.Information)
            {
                OutputInformationalMessage(state, exception, formatter);

                return;
            }

            this.OutputMessageWithStatus(logLevel, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.Debug;
        }

        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        private void OutputMessageWithStatus<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            string msg = formatter(state, exception);

            Action<string> output = Console.WriteLine;

            if (IsError(logLevel))
            {
                Interlocked.Increment(ref this._errors);

                output = Console.Error.WriteLine;
            }

            string status = logLevel.ToString()
                .ToUpperInvariant();

            output($"{status}: {msg}");
        }

        private static void OutputInformationalMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = formatter(state, exception);
            Console.WriteLine(msg);
        }

        private static bool IsError(LogLevel logLevel)
        {
            return logLevel == LogLevel.Critical || logLevel == LogLevel.Error;
        }

        private bool IsWarningAsError(LogLevel logLevel)
        {
            return this._warningsAsErrors && logLevel == LogLevel.Warning;
        }
    }
}