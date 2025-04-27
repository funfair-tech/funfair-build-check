#if NET7_0_OR_GREATER
#else
using System;
using System.Text.RegularExpressions;

namespace FunFair.BuildCheck.Runner;

internal static partial class SourceGenerated
{
    private static readonly TimeSpan TimeoutTimeSpan = TimeSpan.FromMilliseconds(TIMEOUT_MILLISECONDS);

    private static readonly Regex RegexProjectReference = new(
        pattern: PROJECT_REFERENCE_REGEX,
        options: PROJECT_REFERENCE_OPTIONS,
        matchTimeout: TimeoutTimeSpan
    );

    public static Regex ProjectReferenceRegex()
    {
        return RegexProjectReference;
    }
}
#endif
