using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner.Tests.Helpers;

internal sealed class CapturingLogger : ILogger
{
    private readonly bool _isEnabled;
    private readonly List<LogLevel> _levels = [];

    public CapturingLogger(bool isEnabled = true)
    {
        this._isEnabled = isEnabled;
    }

    public IReadOnlyList<LogLevel> Levels => this._levels;

    public bool BeginScopeCalled { get; private set; }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        this.BeginScopeCalled = true;

        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return this._isEnabled;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        this._levels.Add(logLevel);
    }
}
