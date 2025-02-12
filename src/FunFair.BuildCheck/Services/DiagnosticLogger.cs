using System;
using FunFair.BuildCheck.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Services;

internal sealed class DiagnosticLogger : IDiagnosticLogger
{
    private readonly bool _warningsAsErrors;

    public DiagnosticLogger(bool warningsAsErrors)
    {
        this._warningsAsErrors = warningsAsErrors;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (this.IsWarningAsError(logLevel))
        {
            this.OutputErrorMessage(eventId: eventId, state: state, exception: exception, formatter: formatter);

            return;
        }

        if (logLevel == LogLevel.Information)
        {
            OutputInformationalMessage(state: state, exception: exception, formatter: formatter);

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

        string msg = formatter(arg1: state, arg2: exception);

        Action<string> output = IsError(logLevel)
            ? Console.Error.WriteLine
            : Console.WriteLine;

        string status = logLevel.GetName()
                                .ToUpperInvariant();

        output($"{status}: {msg}");
    }

    private static void OutputInformationalMessage<TState>(TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string msg = formatter(arg1: state, arg2: exception);
        Console.WriteLine(msg);
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