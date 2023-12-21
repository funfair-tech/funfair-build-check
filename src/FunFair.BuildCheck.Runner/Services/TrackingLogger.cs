using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner.Services;

internal sealed class TrackingLogger : ITrackingLogger
{
    private readonly ILogger _logger;
    private readonly bool _warningsAsErrors;
    private long _errors;

    public TrackingLogger(bool warningsAsErrors, ILogger logger)
    {
        this._warningsAsErrors = warningsAsErrors;
        this._logger = logger;
    }

    public long Errors => this._errors;

    public bool IsErrored => this.Errors > 0;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)

    {
        if (this.IsWarningAsError(logLevel))
        {
            this.OutputErrorMessage(eventId: eventId, state: state, exception: exception, formatter: formatter);

            return;
        }

        this.OutputMessageWithStatus(logLevel: logLevel, state: state, exception: exception, formatter: formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.Debug;
    }

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        return new DisposableScope();
    }

    private void OutputErrorMessage<TState>(in EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        this.Log(logLevel: LogLevel.Error, eventId: eventId, state: state, exception: exception, formatter: formatter);
    }

    private void OutputMessageWithStatus<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        if (IsError(logLevel))
        {
            Interlocked.Increment(ref this._errors);
        }

        this._logger.Log(logLevel: logLevel, eventId: 0, state: state, exception: exception, formatter: formatter);
    }

    private static bool IsError(LogLevel logLevel)
    {
        return logLevel is LogLevel.Critical or LogLevel.Error;
    }

    private bool IsWarningAsError(LogLevel logLevel)
    {
        return this._warningsAsErrors && logLevel == LogLevel.Warning;
    }

    private sealed class DisposableScope : IDisposable
    {
        public void Dispose()
        {
            // Nothing to do here.
        }
    }
}