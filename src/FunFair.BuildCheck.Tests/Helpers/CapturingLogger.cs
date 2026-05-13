using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Tests.Helpers;

internal sealed class CapturingLogger<T> : ILogger<T>
{
    private readonly List<CapturedLogEntry> _entries = [];

    public IReadOnlyList<CapturedLogEntry> Entries => this._entries;

    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        this._entries.Add(new(Level: logLevel, EventId: eventId, Message: formatter(arg1: state, arg2: exception)));
    }
}
